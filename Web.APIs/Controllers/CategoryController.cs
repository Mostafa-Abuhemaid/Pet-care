using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Web.Application.DTOs.CategoryDTO;
using Web.Application.Interfaces;
using Web.Domain.Entites;
using Web.Infrastructure.Service;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _CategoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _CategoryService = categoryService;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAllCategoryAsync()
        {

            var categories = await _CategoryService.GetAllCategory();
            
            return Ok(categories);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromQuery]SendCategoryDTO categoryDTO)
        {
            var category = await _CategoryService.CreateCategoryAsync(categoryDTO);
           return Ok(category);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _CategoryService.DeleteCategoryAsync(id);

            return category.Success ? Ok(category) : BadRequest(category);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateCategory(int Id,[FromQuery] SendCategoryDTO categoryDTO)
        {
            var category = await _CategoryService.UpdateCategoryAsync(Id,categoryDTO);
            return category.Success ? Ok(category) : BadRequest(category);
        }

    }
}
