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

        public async Task<BaseResponse<VetDetailsDto>> AddAsync(VetRequest request)
        {
            if (await _context.VetClinics.AnyAsync(x => x.Name == request.Name))
                return new BaseResponse<VetDetailsDto>(false, "Vet is Already Exist!");

            //toDOO   ADD userid for add addresss


            var entity = request.Adapt<VetClinic>();
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
            if (await _context.VetClinics.AnyAsync(x => x.Id == id &&!x.Deleted))
                return new BaseResponse<VetDetailsDto>(false, $"Vet with {id} is Deleted !");

            var vet = await _context.VetClinics
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
            if (await _context.VetClinics.SingleOrDefaultAsync(x => x.Id == id&&!x.Deleted|| x.Name == request.Name) is not { } vet)
                return new BaseResponse<bool>(false, $"Vet with {request.Name} is Deleted or Another Vet with the same name already exists !");

            string newPhotoUrl = vet.logoUrl!;
            if (request.Photo != null)
            {
                newPhotoUrl = Files.UploadFile(request.Photo, "Vet");
                if (!string.IsNullOrEmpty(vet.logoUrl))
                    Files.DeleteFile(vet.logoUrl, "Vet");
            }

            await _context.VetClinics.Where(x => x.Id == id)
  .ExecuteUpdateAsync(setters =>
        setters.SetProperty(x => x.Name, request.Name)
               .SetProperty(x => x.Description, request.Description)
               .SetProperty(x => x.Type, request.Type)
               .SetProperty(x => x.Phone, request.Phone)
               .SetProperty(x => x.Services, request.Services)
               .SetProperty(x => x.Experience, request.Experience)
               .SetProperty(x => x.IsEmergencyAvailable, request.IsEmergencyAvailable)
               .SetProperty(x => x.Address.City, request.Address.City)
               .SetProperty(x => x.Address.Country, request.Address.Country)
               .SetProperty(x => x.Address.Street, request.Address.Street)
               .SetProperty(x => x.PricePerNight, request.PricePerNight)
               .SetProperty(x => x.CountOfPatients, request.CountOfPatients));
            return new BaseResponse<bool>(true, "Updated successfully.", true);
        }
    }
}
