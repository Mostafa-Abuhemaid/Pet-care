using Mapster;
using Microsoft.EntityFrameworkCore;
using PetCare.Api.Entities;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.Files;
using Web.Application.Response;
using Web.Infrastructure.Data;

namespace Web.Infrastructure.Service
{
    public class BasePetService<T> where T : Pet, new()
    {
        private readonly AppDbContext _context;

        public BasePetService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<PetResponse>> AddAsync(PetRequest request, string userId, CancellationToken cancellationToken = default)
        {
            if (request == null)
                return new BaseResponse<PetResponse>(false, "Request is required.");

            var petExists = await _context.Set<T>()
                .AnyAsync(p => p.AppUserId == userId && p.Name == request.Name, cancellationToken);

            if (petExists)
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
            if (request == null)
                return new BaseResponse<bool>(false, "Request is required.");

            var item = await _context.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id && x.AppUserId == userId, cancellationToken);

            if (item == null)
                return new BaseResponse<bool>(false, $"Pet with ID {id} was not found for this user.");

            var anotherPetWithSameName = await _context.Set<T>()
    .AnyAsync(p => p.Id != id && p.AppUserId == userId && p.Name == request.Name, cancellationToken);

            if (anotherPetWithSameName)
                return new BaseResponse<bool>(false, "Another pet with the same name already exists.");

            request.Adapt(item);

            if (request.Photo != null)
                item.PhotoUrl = Files.UploadFile(request.Photo, "Pet");

            await _context.SaveChangesAsync(cancellationToken);
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
    }
}
