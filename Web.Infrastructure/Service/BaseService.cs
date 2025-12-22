using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Web.Application.Common;
using Web.Application.Files;
using Web.Application.Response;

namespace Web.Infrastructure.Service
{
    public class BaseService<TEntity, TRequest, TDto>
        where TEntity : class, new()
    {
        protected readonly DbContext _context;

        public BaseService(DbContext context)
        {
            _context = context;
        }

        public virtual async Task<BaseResponse<TDto>> AddAsync(
            TRequest request,
            string folderName = null,
            IFormFile photo = null)
        {
            var dbSet = _context.Set<TEntity>();
            var entity = request.Adapt<TEntity>();

            // Check duplicate Name (if exists)
            var nameProp = typeof(TEntity).GetProperty("Name")?.GetValue(entity)?.ToString();
            if (!string.IsNullOrWhiteSpace(nameProp) &&
                await dbSet.AnyAsync(x => EF.Property<string>(x, "Name") == nameProp))
            {
                return new BaseResponse<TDto>(false, $"{typeof(TEntity).Name} already exists!");
            }

            // Upload photo if exists
            if (photo != null && !string.IsNullOrWhiteSpace(folderName))
            {
                typeof(TEntity).GetProperty("logoUrl")?.SetValue(entity, Files.UploadFile(photo, folderName));
            }

            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            var dto = entity.Adapt<TDto>();
            return new BaseResponse<TDto>(true, "Created successfully.", dto);
        }

        public virtual async Task<BaseResponse<bool>> UpdateAsync(
            int id,
            TRequest request,
            Action<TEntity, TRequest> mapUpdates)
        {
            var dbSet = _context.Set<TEntity>();
            var entity = await dbSet.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);

            if (entity == null)
                return new BaseResponse<bool>(false, $"{typeof(TEntity).Name} with {id} not found.");

            mapUpdates(entity, request);
            await _context.SaveChangesAsync();

            return new BaseResponse<bool>(true, "Updated successfully.", true);
        }

        public virtual async Task<BaseResponse<bool>> DeleteAsync(int id)
        {
            var dbSet = _context.Set<TEntity>();
            var entity = await dbSet.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);

            if (entity == null)
                return new BaseResponse<bool>(false, $"{typeof(TEntity).Name} not found.");

            var deletedProp = typeof(TEntity).GetProperty("Deleted");
            if (deletedProp != null)
            {
                deletedProp.SetValue(entity, true);
            }

            await _context.SaveChangesAsync();
            return new BaseResponse<bool>(true, "Deleted successfully.", true);
        }

        public virtual async Task<BaseResponse<TDto>> GetAsync(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            var dbSet = _context.Set<TEntity>().AsQueryable();
            foreach (var include in includes)
                dbSet = dbSet.Include(include);

            var entity = await dbSet.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
            if (entity == null)
                return new BaseResponse<TDto>(false, $"{typeof(TEntity).Name} not found.");

            var dto = entity.Adapt<TDto>();
            return new BaseResponse<TDto>(true, "Success", dto);
        }

        public virtual async Task<BaseResponse<PaginatedList<TDto>>> GetAllAsync(
            RequestFilters filters,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeLogic = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (includeLogic != null)
                query = includeLogic(query);

            if (!string.IsNullOrWhiteSpace(filters.SearchValue))
            {
                query = query.Where(x =>
                    EF.Property<string>(x, "Name").Contains(filters.SearchValue) ||
                    EF.Property<string>(x, "Description").Contains(filters.SearchValue));
            }

            var result = await PaginatedList<TDto>.CreateAsync(
                query.ProjectToType<TDto>(),
                filters.PageNumber,
                filters.PageSize);

            return new BaseResponse<PaginatedList<TDto>>(true, "Success", result);
        }
    }
}
