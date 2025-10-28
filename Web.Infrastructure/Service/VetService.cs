using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetCare.Api.Entities;
using QuestPDF.Fluent;
using Stripe;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Web.Application.Common;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.DTOs.VetDTO;
using Web.Application.DTOs.VetDTO.DownloadReceipt;
using Web.Application.Files;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Domain.Entites;
using Web.Domain.Enums;
using Web.Infrastructure.Persistence.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Web.Infrastructure.Service
{
    public class VetService(AppDbContext context) : IVetService
    {
        private readonly AppDbContext _context = context;
       

        public async Task<BaseResponse<VetDetailsDto>> AddAsync(string userId, VetRequest request)
        {
            if (await _context.VetClinics.AnyAsync(x => x.Name == request.Name))
                return new BaseResponse<VetDetailsDto>(false, "Vet is Already Exist!");

            var entity = request.Adapt<VetClinic>();
            entity.Address.UserId = userId;
            if (request.Services != null && request.Services.Any())
            {
                entity.Services = request.Services
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => new VetClinicService { Name = s.Trim() })
                    .ToList();
            }

            if (request.Photo != null)
                entity.logoUrl = Files.UploadFile(request.Photo, "Vet");
            await _context.VetClinics.AddAsync(entity);
            await _context.SaveChangesAsync();

            var response = entity.Adapt<VetDetailsDto>();
            return new BaseResponse<VetDetailsDto>(true, "Created successfully.", response);
        }

        public async Task<BaseResponse<bool>> DeleteAsync(int id)
        {
            if (await _context.VetClinics.SingleOrDefaultAsync(x => x.Id == id && !x.Deleted) is not { } vet)
                return new BaseResponse<bool>(false, "Vet is Already Deleted !");

            vet.Deleted = true;
            await _context.SaveChangesAsync();
            return new BaseResponse<bool>(true, "Success!", true);
        }

        public async Task<BaseResponse<PaginatedList<VetListItemDto>>> GetAllAsync(RequestFilters filters = default!, AddtionalRequestFilters addtionalRequestFilters = default!)
        {
            var query = _context.VetClinics
                 .Include(x => x.Address)
                 .Include(x => x.vetSchedules)
                 .Include(x => x.appointments)
                 .Include(x => x.Reviews)
                 .ThenInclude(x => x.AppUser)
                 .Include(x => x.Services)
                 .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.SearchValue))
                query = SearchResults(filters, query);

            query= AddtionalFilters(addtionalRequestFilters, query);

            if (!string.IsNullOrWhiteSpace(filters.SortColumn))
                query = SortQuery(filters, query);

            var paginatedVets = await createPaginatedList(filters, query);  

            return new BaseResponse<PaginatedList<VetListItemDto>>(true, "Success", paginatedVets);

        }

        public async Task<BaseResponse<VetDetailsDto>> GetAsync(int id)
        {
            var vet = await _context.VetClinics
                .Include(x => x.Services)
                .Include(x => x.Address)
                .Include(x => x.vetSchedules)
                .Include(x => x.appointments)
                .Include(x => x.Reviews).ThenInclude(r => r.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (vet == null)
                return new BaseResponse<VetDetailsDto>(false, $"Vet with {id} is Not Found !");

            if (vet.Deleted)
                return new BaseResponse<VetDetailsDto>(false, $"Vet with {id} is Deleted !");

            var response = new VetDetailsDto(
                vet.Id,
                vet.Name,
                vet.Description,
                vet.Phone,
                vet.logoUrl,
                vet.Type.ToString(),   // Enum → String
                vet.Services.Select(s => s.Name).ToList(),
                $"{vet.Address.Country}/{vet.Address.City}/{vet.Address.Street}",
                vet.PricePerNight,
                vet.IsEmergencyAvailable,
                vet.Experience,
                vet.CountOfPatients,
                vet.Reviews.Any() ? vet.Reviews.Average(r => r.Rating) : 0,
                vet.Reviews.Count,
                vet.Reviews.Select(r => new VetUserReviewDto (
                    r.AppUser.Id,
                    r.AppUser.FullName,
                    r.Comment,
                    r.Createdon
                )).ToList(),
                vet.vetSchedules.Select(s => new VetScheduleDto(
                    s.DayOfWeek.ToString(),
                    s.StartTime,
                    s.EndTime
                )).ToList()
            );

            return new BaseResponse<VetDetailsDto>(true, "Success!", response);
        }




        public async Task<BaseResponse<List<AvailableSlotDto>>> GetAvailableSlotsAsync(int VetId, GetAvailableSlotsRequest request)
        {
            var schedule = await _context.VetSchedule
                .Include(x => x.vetClinic)
                .ThenInclude(x=>x.Address)
                .FirstOrDefaultAsync(s => s.VetClinicId == VetId
                                       && s.DayOfWeek == (DayOfWeekEnum)request.Date.DayOfWeek);

            if (schedule == null)
                return new BaseResponse<List<AvailableSlotDto>>(true, "Success", new List<AvailableSlotDto>());

            var booked = await _context.Appointments
                .Where(a => a.VetClinicId == VetId
                         && a.Date.Date == request.Date.ToDateTime(TimeOnly.MinValue).Date)
                .Select(a => a.StartTime)
                .ToListAsync();

            var slots = new List<AvailableSlotDto>();
            var current = request.Date.ToDateTime(schedule.StartTime);
            var end = request.Date.ToDateTime(schedule.EndTime);

            while (current < end)
            {
                var next = current.AddMinutes(60); // slot duration
                var isBooked = booked.Contains(TimeOnly.FromDateTime(current));


                slots.Add(new AvailableSlotDto(
                    request.Date,
                    current,
                    next,
                    schedule.vetClinic.Address.Adapt<AddressDto>(),
                    schedule.vetClinic.PricePerNight,
                    isBooked
                ));

                current = next;
            }


            var available = slots.Where(s => !s.IsBooked).ToList();
            return new BaseResponse<List<AvailableSlotDto>>(true, "Success", available);
        }


        public async Task<BaseResponse<bool>> UpdateAsync(int id, VetRequest request)
        {
            var vet = await _context.VetClinics
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);

            if (vet is null)
                return new BaseResponse<bool>(false, $"Vet with {id} not found or is deleted!");

            if (await _context.VetClinics.AnyAsync(x => x.Id != id && x.Name == request.Name))
                return new BaseResponse<bool>(false, $"Another Vet with the same name already exists!");

            string newPhotoUrl = vet.logoUrl;
            if (request.Photo != null)
            {
                newPhotoUrl = Files.UploadFile(request.Photo, "Vet");
                if (!string.IsNullOrEmpty(vet.logoUrl))
                    Files.DeleteFile(vet.logoUrl, "Vet");
            }

            vet.Name = request.Name;
            vet.Description = request.Description;
            vet.Type = request.Type;
            vet.Phone = request.Phone;
            vet.Experience = request.Experience;
            vet.IsEmergencyAvailable = request.IsEmergencyAvailable;
            vet.PricePerNight = request.PricePerNight;
            vet.CountOfPatients = request.CountOfPatients;
            vet.logoUrl = newPhotoUrl;
            vet.Services = request.Services
                 .Where(s => !string.IsNullOrWhiteSpace(s))
                 .Select(s => new VetClinicService { Name = s.Trim() })
                 .ToList();

            vet.Address.City = request.Address.City;
            vet.Address.Country = request.Address.Country;
            vet.Address.Street = request.Address.Street;

            await _context.SaveChangesAsync();

            return new BaseResponse<bool>(true, "Updated successfully.", true);
        }

        public async Task <BaseResponse<VetReviewsDto>>GetReviewsasync(int VetId)
        {
            var vet = await _context.VetClinics
              .Include(x => x.Reviews)
              .ThenInclude(x => x.AppUser)
              .FirstOrDefaultAsync(x => x.Id == VetId);

           
       var vetUserReviewList = vet.Reviews
    .Select(r => new VetUserReviewDto(
        r.AppUserId,
        r.AppUser.FullName,
        r.Comment,
        r.DatePosted
    ))
    .ToList();


            var response = new VetReviewsDto
            (
                vet.Id,
                vet.Reviews.Average(x => x.Rating),
                vet.Reviews.Count(),
                vetUserReviewList
            );


            return new BaseResponse<VetReviewsDto>(true,"Success",response);
        }

        public async Task<BaseResponse<VetBookingReceiptDTO>> BookingVet(string userId,int VetclinicId, BookVetDTO request)
        {
            var vet = await _context.VetClinics
                .Include(c => c.Address)
                .FirstOrDefaultAsync(x => x.Id == VetclinicId);

            if (vet == null)
                return new BaseResponse<VetBookingReceiptDTO>(false, "Vet clinic not found");

            var existingBooking = await _context.vetBookings
        .AnyAsync(b => b.UserId == userId
                && b.PetId == request.PetId
                && b.VetClinicId == VetclinicId
                && b.Date == request.Date
                && b.Time == request.Time);

            if (existingBooking)
            {
                return new BaseResponse<VetBookingReceiptDTO>(
                    false,
                    "You already have a booking for this pet at the same clinic and time."
                );
            }



            var clinicServices = await _context.VetClinicService
                .Where(s => s.VetClinicId == VetclinicId)
                .Select(s => s.Id)
                .ToListAsync();

      
            var validServiceIds = request.ServiceIds
                .Where(id => clinicServices.Contains(id))
                .ToList();

            var vetbooking = new VetBooking
            {
                UserId = userId,
                PetId = request.PetId,
                VetClinicId = VetclinicId,
                Date = request.Date,
                Time = request.Time,
                Price = vet.PricePerNight,
                Status = BookingStatus.Pending,
                ReceiptNumber = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(),
                VetBookingServices = validServiceIds
                    .Select(serviceId => new VetBookingService
                    {
                        VetClinicServiceId = serviceId
                    }).ToList()
            };

            await _context.vetBookings.AddAsync(vetbooking);
            await _context.SaveChangesAsync();

            // Reload booking with includes
            var loadedBooking = await _context.vetBookings
                .Include(b => b.Pet)
                .Include(b => b.VetClinic).ThenInclude(c => c.Address)
                .Include(b => b.VetBookingServices).ThenInclude(vbs => vbs.VetClinicService)
                .FirstOrDefaultAsync(b => b.Id == vetbooking.Id);

            var response = new VetBookingReceiptDTO(
                loadedBooking.ReceiptNumber,
                loadedBooking.Id,
                loadedBooking.Pet.Name,
                loadedBooking.VetClinic.Name,
                loadedBooking.Date,
                loadedBooking.Time,
                loadedBooking.Price,
                loadedBooking.VetBookingServices.Select(x => x.VetClinicService.Name).ToList(),
                $"{loadedBooking.VetClinic.Address.Country}/{loadedBooking.VetClinic.Address.City}/{loadedBooking.VetClinic.Address.Street}",
                "Instructions / Terms:\r\n-Kindly note ..."
            );

            return new BaseResponse<VetBookingReceiptDTO>(true, "Success", response);
        }

        public async Task<BaseResponse<bool>> ConfirmBookingAsync(string userid, int bookingId)
        {
            var booking = await _context.vetBookings.Include(x=>x.VetClinic)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                return new BaseResponse<bool>(false, "Booking not found");

            if (booking.Status == BookingStatus.Confirmed)
                return new BaseResponse<bool>(false, "Booking already confirmed");

            booking.Status = BookingStatus.Confirmed;
             booking.Updatedon = DateTime.UtcNow; 
            _context.vetBookings.Update(booking);

            var history = new History
            {
                Name=booking.VetClinic.Name,
                Desciption=booking.VetClinic.Description,
                Price=booking.VetClinic.PricePerNight,
                Unit="Session",
                Date = DateTime.Now,
                UserId = userid,
                VetClinicId = booking.VetClinicId
            };

            _context.histories.Add(history);
            await _context.SaveChangesAsync();

            return new BaseResponse<bool>(true, "Booking confirmed successfully", true);
        }


        public async Task<BaseResponse<ReceiptPdfResult?>> GenerateVetReceiptPdfByBookingIdAsync(int bookingId)
        {
            var booking = await _context.vetBookings
                .Include(b => b.Pet)
                .Include(b => b.VetClinic).ThenInclude(c => c.Address)
                .Include(b => b.VetBookingServices).ThenInclude(vbs => vbs.VetClinicService)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                return new BaseResponse<ReceiptPdfResult?>(false,"Booking not found ");

            var dto = MapToDto(booking);

            var document = new VetReceiptDocument(dto); 
            using var ms = new MemoryStream();
            document.GeneratePdf(ms);
            var response= new ReceiptPdfResult
            {
                Content = ms.ToArray(),
                FileName = $"Receipt_{dto.ReceiptNumber}.pdf"
            };
            return new BaseResponse<ReceiptPdfResult?>(true, "Success", response);
        }


        private IQueryable<VetClinic> SearchResults(RequestFilters filters, IQueryable<VetClinic> query)
        {
            return query.Where(x => x.Name.Contains(filters.SearchValue)
                                      || x.Description.Contains(filters.SearchValue)
                                      || x.Address.City.Contains(filters.SearchValue)
                                      || x.Address.Country.Contains(filters.SearchValue));

        }

        private IQueryable<VetClinic> SortQuery(RequestFilters filters, IQueryable<VetClinic> query)
        {

            return filters.SortColumn?.ToLower() switch
            {
                "popularity" => query.OrderByDescending(x => x.appointments.Count),
                "rating" => query.OrderByDescending(x => x.Reviews.Any() ? x.Reviews.Average(r => r.Rating) : 0),
                "nearest" => query.OrderBy(x => x.Address.City), //ToDo calculate distance
                "pricelowtohigh" => query.OrderBy(x => x.PricePerNight),
                "pricehightolow" => query.OrderByDescending(x => x.PricePerNight),
                _ => query.OrderBy(x => x.Id) // Default sorting
            };
            
        }
        private async Task<PaginatedList<VetListItemDto>> createPaginatedList(RequestFilters filters, IQueryable<VetClinic> query)
        {
            var result = await PaginatedList<VetListItemDto>.CreateAsync(
            query.Select(x => new VetListItemDto(
                x.Id,
                x.Name,
                x.Type,
                x.logoUrl,
                x.PricePerNight,
                x.Reviews.Select(r => (double?)r.Rating).Average() ?? 0,
                x.Reviews.Count,
                x.Address.City + " / " + x.Address.Street,
                x.Services.Select(s => s.Name).ToList(),
                x.IsEmergencyAvailable
            )),

                filters.PageNumber,
                filters.PageSize
            );

            return result;
        }

        private IQueryable<VetClinic> AddtionalFilters(AddtionalRequestFilters filters, IQueryable<VetClinic> query)
        {
            if (!string.IsNullOrEmpty(filters.Location))
            {
                query = query.Where(x => x.Address.City.Contains(filters.Location)||x.Address.Street.Contains(filters.Location));
            }

            if (filters.OpenNow == true)
            {
     var now = TimeOnly.FromDateTime(DateTime.Now);
query = query.Where(x => x.vetSchedules.Any(s =>
    s.StartTime <= now && s.EndTime >= now));

            }

            if (filters.PriceMin.HasValue)
            {
                query = query.Where(x => x.PricePerNight >= filters.PriceMin.Value);
            }
            if (filters.PriceMax.HasValue)
            {
                query = query.Where(x => x.PricePerNight <= filters.PriceMax.Value);
            }
            var servicesFilter = filters.Services?
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim().ToLower())
                .ToList();

            if (servicesFilter?.Any() == true)
            {
                query = query.Where(x => x.Services.Any(s => servicesFilter.Contains(s.Name.ToLower())));
            }

            return query;

        }

        private VetBookingReceiptDTO MapToDto(VetBooking booking)
        {
            return new VetBookingReceiptDTO(
                booking.ReceiptNumber,
                booking.Id,
                booking.Pet?.Name ?? "—",
                booking.VetClinic?.Name ?? "—",
                booking.Date,
                booking.Time,
                booking.Price,
                booking.VetBookingServices?.Select(x => x.VetClinicService?.Name ?? "—").ToList() ?? new List<string>(),
                $"{booking.VetClinic?.Address?.Country}/{booking.VetClinic?.Address?.City}/{booking.VetClinic?.Address?.Street}",
                "Instructions / Terms:\r\n-Kindly note ..."
            );
        }

    }
}
