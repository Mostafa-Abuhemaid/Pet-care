using Mapster;
using Microsoft.EntityFrameworkCore;
using PetCare.Api.Entities;
using Stripe;
using System;
using System.Collections.Generic;
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
using Web.Infrastructure.Persistence.Data;

namespace Web.Infrastructure.Service
{
    public class VetService(AppDbContext context) : IVetService
    {
        private readonly AppDbContext _context = context;

        public async Task<BaseResponse<VetDetailsDto>> AddAsync(string userId,VetRequest request)
        {
            if (await _context.VetClinics.AnyAsync(x => x.Name == request.Name))
                return new BaseResponse<VetDetailsDto>(false, "Vet is Already Exist!");

            var entity = request.Adapt<VetClinic>();
            entity.Address.UserId=userId;
            if (request.Photo != null)
                entity.logoUrl = Files.UploadFile(request.Photo, "Vet");
            await _context.VetClinics.AddAsync(entity);
            await _context.SaveChangesAsync();

            var response = entity.Adapt<VetDetailsDto>();
            return new BaseResponse<VetDetailsDto>(true, "Created successfully.", response);
        }

        public async Task<BaseResponse<bool>> DeleteAsync(int id)
        {
            if (await _context.VetClinics.SingleOrDefaultAsync(x => x.Id == id&&!x.Deleted) is not { } vet)
                return new BaseResponse<bool>(false, "Vet is Already Deleted !");

            vet.Deleted = true;
            await _context.SaveChangesAsync();
            return new BaseResponse<bool>(true, "Success!", true);
        }

        public Task<BaseResponse<PaginatedList<VetListItemDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<VetDetailsDto>> GetAsync(int id)
        {
            if (!await _context.VetClinics.AnyAsync(x => x.Id == id))
                return new BaseResponse<VetDetailsDto>(false, $"Vet with {id} is Not Found !");

            if (await _context.VetClinics.AnyAsync(x => x.Id == id &&x.Deleted))
                return new BaseResponse<VetDetailsDto>(false, $"Vet with {id} is Deleted !");

            var vet = await _context.VetClinics
                .Include(x=>x.Address)
                .Include(x => x.vetSchedules)
                .Include(x => x.appointments)
                .Include(x => x.Reviews)
                .ThenInclude(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id);
            var response=vet.Adapt<VetDetailsDto>();
          
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
            vet.Services = request.Services;
            vet.Experience = request.Experience;
            vet.IsEmergencyAvailable = request.IsEmergencyAvailable;
            vet.PricePerNight = request.PricePerNight;
            vet.CountOfPatients = request.CountOfPatients;
            vet.logoUrl = newPhotoUrl;

            vet.Address.City = request.Address.City;
            vet.Address.Country = request.Address.Country;
            vet.Address.Street = request.Address.Street;

            await _context.SaveChangesAsync();

            return new BaseResponse<bool>(true, "Updated successfully.", true);
        }

    }
}
