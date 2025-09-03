using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Application.DTOs.CartDTO;
using Web.Application.Interfaces;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoCodesController(IPromoCodeService promoCodeService) : ControllerBase
    {
        private readonly IPromoCodeService _promoCodeService = promoCodeService;

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreatePromoCodeDto dto)
        {
            var result = await _promoCodeService.CreatePromoCodeAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var result=await _promoCodeService.GetAllPromoCodeAsync();
            return Ok(result);  
        }

        [HttpDelete("/{codeid}")]
        public async Task<IActionResult> Delete([FromRoute]int codeid)
        {
          var result=await _promoCodeService.DeletePromoCodeAsync(codeid);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("/{codeid}")]
        public async Task<IActionResult> ToggelStatusActive([FromRoute] int codeid)
        {
            var result = await _promoCodeService.ToggelStatusActive(codeid);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
