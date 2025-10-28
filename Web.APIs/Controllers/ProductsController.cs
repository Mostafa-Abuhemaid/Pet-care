
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Common;
using Web.Application.DTOs.ProductDTO;
using Web.Application.Interfaces;
using Web.Application.Response;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        [HttpGet("")]
        public async Task<IActionResult> GetProducts([FromQuery] RequestFilters? filters = null)
        {
            var result = await _productService.GetAllAsync(filters!);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute]int id)
        {
            var result = await _productService.GetAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("best-sellers")]
        public async Task<IActionResult> GetBestSellers([FromQuery] RequestFilters? filters = null)
        {
            var result = await _productService.GetBestSellerProductsAsync(filters!);
            return Ok(result);
        }

        [HttpGet("special-offers")]
        public async Task<IActionResult> GetSpecialOffers()
        {
            var result = await _productService.GetSpecialOffersAsync();
            return Ok(result);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory([FromRoute]int categoryId, [FromQuery] RequestFilters? filters = null)
        {
            var result = await _productService.GetAllAsync(filters!, categoryId );
            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommendedProducts([FromQuery] RequestFilters? filters = null)
        {
            var result = await _productService.GetBestSellerProductsAsync(filters);
            return Ok(result);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] string Query )
        {
            var result = await _productService.Search(Query);
            return Ok(result);
        }
    }
}