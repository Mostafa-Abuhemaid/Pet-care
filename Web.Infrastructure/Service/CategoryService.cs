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

namespace Web.Infrastructure.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _Context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _Context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Category>> CreateCategoryAsync(SendCategoryDTO categoryDTO)
        {
            var imgName = Files.UploadFile(categoryDTO.Image, "Category");
            if (string.IsNullOrEmpty(imgName))
            {
                throw new System.Exception("File upload failed.");
            }

            var category = new Category
            {
                Name = categoryDTO.Name,
                ImageUrl = imgName
            };

            await _Context.categories.AddAsync(category);
            await _Context.SaveChangesAsync();

            return new BaseResponse<Category>(true, "Category was Added Successfully", category);
        }

        public async Task<BaseResponse<bool>> DeleteCategoryAsync(int categoryId)
        {
            var category = await _Context.categories.FindAsync(categoryId);

            if (category != null)
            {
                _Context.categories.Remove(category);
                await _Context.SaveChangesAsync();

                Files.DeleteFile(category.ImageUrl, "Category");
                return new BaseResponse<bool>(true, "Category was deleted successfully");
            }
            return new BaseResponse<bool>(false, "Category not found");
        }

        public async Task<BaseResponse<List<GetCategoryDTO>>> GetAllCategory()
        {
            var categories = await _Context.categories.ToListAsync();

            var catDtos = categories.Select(c => new GetCategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = $"{_configuration["BaseURL"]}/Images/Category/{c.ImageUrl}"
            }).ToList();

            return new BaseResponse<List<GetCategoryDTO>>(true, "Categories retrieved successfully", catDtos);
        }

        public async Task<BaseResponse<Category>> UpdateCategoryAsync(int Id, SendCategoryDTO categoryDTO)
        {
            var category = await _Context.categories.FindAsync(Id);

            if (category == null)
                return new BaseResponse<Category>(false, "Category not found");

            category.Name = categoryDTO.Name;

  
            if (categoryDTO.Image != null)
            {         
                Files.DeleteFile(category.ImageUrl, "Category");

                var imgName = Files.UploadFile(categoryDTO.Image, "Category");

                category.ImageUrl = imgName;
            }

            _Context.categories.Update(category);
            await _Context.SaveChangesAsync();

            return new BaseResponse<Category>(true, "Category updated successfully", category);
        }
    }
}
