using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetCare.Api.Entities;
using Stripe;
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
using Web.Application.Files;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Domain.Entites;
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
            if (!await _context.VetClinics.AnyAsync(x => x.Id == id))
                return new BaseResponse<VetDetailsDto>(false, $"Vet with {id} is Not Found !");

            if (await _context.VetClinics.AnyAsync(x => x.Id == id && x.Deleted))
                return new BaseResponse<VetDetailsDto>(false, $"Vet with {id} is Deleted !");

            var vet = await _context.VetClinics
                .Include(x => x.Services)
                .Include(x => x.Address)
                .Include(x => x.vetSchedules)
                .Include(x => x.appointments)
                .Include(x => x.Reviews)
                .ThenInclude(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id);
            var response = vet.Adapt<VetDetailsDto>();

            return new BaseResponse<VetDetailsDto>(true, "Success!", response);
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
                var now = DateTime.Now.TimeOfDay;
                query = query.Where(x => x.vetSchedules.Any(s => s.StartTime <= now && s.EndTime >= now));
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


    }
}
