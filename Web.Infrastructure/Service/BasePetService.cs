using Azure;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetCare.Api.Entities;
using Web.Application.Common;
using Web.Application.Common.Constants;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.DTOs.ProductDTO;
using Web.Application.Files;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Domain.Entites;
using Web.Infrastructure.Persistence.Data;

namespace Web.Infrastructure.Service
{
    public class BasePetService<T>: IBasePetService where T : Pet
    {
        private readonly AppDbContext _context;
        private readonly IValidator<PetRequest> _validator;
        private readonly IConfiguration _configuration;

        public BasePetService(AppDbContext context, IValidator<PetRequest> validator, IConfiguration configuration)
        {
            _context = context;
            _validator = validator;
            _configuration = configuration;
        }


        public async Task<BaseResponse<PetResponse>> AddAsync(PetRequest request, string userId, CancellationToken cancellationToken = default)
        {

            if( await _context.Set<T>().AnyAsync(p => p.AppUserId == userId && p.Name == request.Name, cancellationToken))
                return new BaseResponse<PetResponse>(false, $"Another {request.Name} with the same name already exists.");

            var entity = request.Adapt<T>();
            entity.AppUserId = userId;
            if (request.Photo != null)
                entity.PhotoUrl = Files.UploadFile(request.Photo, "Pet");
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = entity.Adapt<PetResponse>();
            return new BaseResponse<PetResponse>(true, "Created successfully.", response);
        }
        public async Task<BaseResponse<IEnumerable<PetResponse>>> GetAllAsync(string userId, CancellationToken cancellationToken = default)
        {
            var items = await _context.Set<T>()
                .Where(x => x.AppUserId == userId)
                .AsNoTracking()
                .ProjectToType<PetResponse>()
                .ToListAsync(cancellationToken);

            return new BaseResponse<IEnumerable<PetResponse>>(true, "", items);
        }

        public async Task<BaseResponse<PetResponse>> GetAsync(int id, string userId, CancellationToken cancellationToken = default)
        {
            var item = await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.AppUserId == userId, cancellationToken);

            if (item == null)
                return new BaseResponse<PetResponse>(false, $"Pet with ID {id} was not found for this user.");

            var response = item.Adapt<PetResponse>();
          
            return new BaseResponse<PetResponse>(true, "Pet retrieved successfully.", response);
        }

        public async Task<BaseResponse<bool>> UpdateAsync(int id, PetRequest request, string userId, CancellationToken cancellationToken = default)
        {
            if (await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id && x.AppUserId == userId, cancellationToken) is not { } item)
                return new BaseResponse<bool>(false, $"Pet with ID {id} was not found for this user.");

            if (await _context.Set<T>().AnyAsync(p => p.Id != id && p.AppUserId == userId && p.Name == request.Name, cancellationToken))
                return new BaseResponse<bool>(false, "Another pet with the same name already exists.");
            string newPhotoUrl = item.PhotoUrl!; 
            if (request.Photo != null)
            { 
                newPhotoUrl = Files.UploadFile(request.Photo, "Pet");
            if (!string.IsNullOrEmpty(item.PhotoUrl))
                Files.DeleteFile(item.PhotoUrl, "Pet");
             }

                await _context.Set<T>().Where(x => x.Id == id && x.AppUserId == userId)
      .ExecuteUpdateAsync(setters =>
            setters.SetProperty(x => x.Name, request.Name)
                   .SetProperty(x => x.Breed, request.Breed)
                   .SetProperty(x => x.BirthDay, request.BirthDay)
                   .SetProperty(x => x.breedingRequestStatus, request.breedingRequestStatus)
                   .SetProperty(x => x.Gender, request.Gender)
                   .SetProperty(x => x.Color, request.Color)
                   .SetProperty(x => x.Weight, request.Weight)
                   .SetProperty(x => x.MedicalConditions, request.MedicalConditions)
                   .SetProperty(x => x.PhotoUrl, newPhotoUrl),
            cancellationToken);
            return new BaseResponse<bool>(true, "Updated successfully.", true);
        }

        public async Task<BaseResponse<bool>> DeleteAsync(int id, string userId, CancellationToken cancellationToken = default)
        {
            var item = await _context.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id && x.AppUserId == userId, cancellationToken);

            if (item == null)
                return new BaseResponse<bool>(false, $"Pet with ID {id} was not found for this user.");

            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync(cancellationToken);

            return new BaseResponse<bool>(true, "Deleted successfully.", true);
        }

        public async Task<BaseResponse<PaginatedList<PetMatingResponse>>> AvaliableMatingAsync(
            RequestFilters filters,
            CancellationToken cancellationToken = default)
        {
            var skip = (filters.PageNumber - 1) * filters.PageSize;

            var allPetsQuery =_context.Pet_Dogs
                .Where(d => d.breedingRequestStatus == "intact")
                .Select(d => new { d.Id, d.Name, d.Gender, d.Breed, d.Color, d.PhotoUrl, Type = "Dog" })
                .Concat(_context.Pet_Cats
                    .Where(c => c.breedingRequestStatus == "intact")
                    .Select(c => new { c.Id, c.Name, c.Gender, c.Breed, c.Color, c.PhotoUrl, Type = "Cat" }))
                .OrderBy(p => p.Id)
                .Skip(skip)
                .Take(filters.PageSize);

            var result = await allPetsQuery
                .Select(p => new PetMatingResponse(p.Id, p.Name, p.Gender, p.Breed, p.Color, p.PhotoUrl, p.Type))
                .ToListAsync(cancellationToken);

            var response = new PaginatedList<PetMatingResponse>(
      result,
      result.Count,
      filters.PageNumber,
      filters.PageSize
  );

            return new BaseResponse<PaginatedList<PetMatingResponse>>(
                true,
                "Pets for mating retrieved successfully.",
                response
            );
        }

    }
}
