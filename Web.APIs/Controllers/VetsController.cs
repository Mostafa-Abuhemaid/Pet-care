using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Application.DTOs.VetDTO;
using Web.Application.Interfaces;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VetsController(IVetService vetService) : ControllerBase
    {
        private readonly IVetService _vetService = vetService;

        [HttpPost("Add-New-Vet")]
        public async Task<IActionResult> Add([FromForm] VetRequest request)
        {
            var result=await _vetService.AddAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Get-Vet/{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            var result=await _vetService.GetAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Get-All-Vets")]
        public async Task<IActionResult> GetAll()
        {
            var result=await _vetService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-Vet/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] VetRequest request)
        {
            var result = await _vetService.UpdateAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("Delete-Vet/{id}")]
        public async Task<IActionResult> Add([FromRoute] int id)
        {
            var result=await _vetService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
