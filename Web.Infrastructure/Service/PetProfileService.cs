using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.AccountDTO;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Infrastructure.Data;
using MapsterMapper;
using Mapster;
using PetCare.Api.Entities;
using Web.Application.Files;

namespace Web.Infrastructure.Service
{
    public class PetProfileService(AppDbContext context) : IPetProfileService
    {
        private readonly AppDbContext _context = context;

        public async Task<BaseResponse<PetResponse>> AddAsync(PetRequest request, string UserId, CancellationToken cancellationToken = default)
        {
            if (request is null)
                return new BaseResponse<PetResponse>(false, "Request data is required.");

            var petExists = await _context.Pet_Cats
                .AnyAsync(p => p.AppUserId == UserId && p.Name == request.Name, cancellationToken);

            if (petExists)
                return new BaseResponse<PetResponse>(false, "Another Pet with the same name already exists.");

            var photoPath =Files.UploadFile(request.Photo, "Pet");

            var pet = request.Adapt<Pet_Cat>();
            pet.PhotoUrl = photoPath;
            pet.AppUserId = UserId;

            await _context.Pet_Cats.AddAsync(pet, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new BaseResponse<PetResponse>(true, "Pet added successfully.", pet.Adapt<PetResponse>());
        }

        public async Task<BaseResponse<IEnumerable<PetResponse>>> GetAllAsync(string UserId, CancellationToken cancellationToken = default)
        {
            var pets = await _context.Pet_Cats.Where(p => p.AppUserId == UserId)
               .ProjectToType<PetResponse>()
               .AsNoTracking()
               .ToListAsync();
            return new BaseResponse<IEnumerable<PetResponse>>(true, "", pets);

            throw new NotImplementedException();
        }

        public async Task<BaseResponse<PetResponse>> GetAsync(int id, string UserId, CancellationToken cancellationToken = default)
        {
            var pet = await _context.Pet_Cats
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == id && p.AppUserId == UserId, cancellationToken);

            if (pet is null)
                return new BaseResponse<PetResponse>(false, $"Pet with ID {id} was not found for this user.");

            return new BaseResponse<PetResponse>(true, "Pet retrieved successfully.", pet.Adapt<PetResponse>());
        }

        public async Task<BaseResponse<bool>> UpdateAsync(int id, PetRequest request, string userId, CancellationToken cancellationToken = default)
        {
            if (request is null)
                return new BaseResponse<bool>(false, "Request data is required.");

            var pet = await _context.Pet_Cats.SingleOrDefaultAsync(p => p.Id == id && p.AppUserId == userId, cancellationToken);

            if (pet is null)
                return new BaseResponse<bool>(false, $"Pet with ID {id} was not found for this user.");

            var anotherPetWithSameName = await _context.Pet_Cats
                .AnyAsync(p => p.Id != id && p.AppUserId == userId && p.Name == request.Name, cancellationToken);

            if (anotherPetWithSameName)
                return new BaseResponse<bool>(false, "Another pet with the same name already exists.");


            if (request.Photo != null)
                pet.PhotoUrl = Files.UploadFile(request.Photo, "Pet");
            
            pet.Name = request.Name;
            pet.Breed = request.Breed;
            pet.Age = request.Age;
            pet.Gender = request.Gender;
            
           

            await _context.SaveChangesAsync(cancellationToken);

            return new BaseResponse<bool>(true, "Pet updated successfully.", true);
        }

        public async Task<BaseResponse<bool>> DeleteAsync(int id, string UserId, CancellationToken cancellationToken = default)
        {
            var pet = await _context.Pet_Cats
               .SingleOrDefaultAsync(p => p.Id == id && p.AppUserId == UserId, cancellationToken);

            if (pet is null)
                return new BaseResponse<bool>(false, $"Pet with ID {id} was not found for this user.");

             _context.Pet_Cats.Remove(pet);
            await _context.SaveChangesAsync(cancellationToken);
            return new BaseResponse<bool>(true, "Pet Deleted successfully");

        }




    }
}
