using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.DTOs.CategoryDTO;
using Web.Application.Files;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Domain.Entites;
using Web.Infrastructure.Persistence.Data;
using MapsterMapper;
using Mapster;

namespace Web.Infrastructure.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _Context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext context)
        {
            _Context = context;
        }

        public async Task<BaseResponse<GetCategoryDTO>> CreateCategoryAsync(SendCategoryDTO categoryDTO)
        {
            if (await _Context.categories.AnyAsync(x => x.Name == categoryDTO.Name))
                return new BaseResponse<GetCategoryDTO>(false, "CategoryId is already exists");

            var category = new Category
            {
                Name = categoryDTO.Name,
            };

            await _Context.categories.AddAsync(category);
            await _Context.SaveChangesAsync();
            var response = category.Adapt<GetCategoryDTO>();

            return new BaseResponse<GetCategoryDTO>(true, "CategoryId was Added Successfully", response);
        }

        public async Task<BaseResponse<bool>> DeleteCategoryAsync(int categoryId)
        {
            var category = await _Context.categories.FindAsync(categoryId);

            if (category != null)
            {
                //soft deleted to keep database
                category.Deleted = true; 
                await _Context.SaveChangesAsync();
                return new BaseResponse<bool>(true, "CategoryId was deleted successfully");
            }
            return new BaseResponse<bool>(false, "CategoryId not found");
        }

        public async Task<BaseResponse<List<GetCategoryDTO>>> GetAllCategory()
        {
            var categories = await _Context.categories
                .Where(c=>!c.Deleted)
                .AsNoTracking()
                .ProjectToType<GetCategoryDTO>()
                .ToListAsync();
            
            return new BaseResponse<List<GetCategoryDTO>>(true, "Categories retrieved successfully", categories);
        }

        public async Task<BaseResponse<GetCategoryDTO>> UpdateCategoryAsync(int Id, SendCategoryDTO categoryDTO)
        {
            var category = await _Context.categories.FindAsync(Id);

            if (category == null||category.Deleted)
                return new BaseResponse<GetCategoryDTO>(false, "CategoryId not found");

            category.Name = categoryDTO.Name;

            _Context.categories.Update(category);
            await _Context.SaveChangesAsync();
            var response = category.Adapt<GetCategoryDTO>();

            return new BaseResponse<GetCategoryDTO>(true, "CategoryId updated successfully", response);
        }
    }
}
