using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.Interfaces;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PetsController(IPetProfileService petProfileService) : ControllerBase
    {
        private readonly IPetProfileService _petProfileService = petProfileService;

        [HttpPost("")]
        public async Task<IActionResult>Add([FromBody] PetRequest request,CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result=await _petProfileService.AddAsync(request,userid,cancellationToken);
            return result.Success ? Ok(result) : BadRequest(result);

        }
        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromRoute] int id, CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _petProfileService.GetAllAsync( userid!, cancellationToken);

            return result.Success ? Ok(result) : NotFound(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult>Get([FromRoute]int id,CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result=await _petProfileService.GetAsync(id,userid!,cancellationToken);

            return result.Success ? Ok(result) : NotFound(result);

        }



        [HttpPut("")]
        public async Task<IActionResult>Update([FromRoute]int id, [FromBody] PetRequest request,CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result=await _petProfileService.UpdateAsync(id,request,userid!,cancellationToken);

            return result.Success ? Ok(result) : BadRequest(result);

        }
        [HttpDelete("")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _petProfileService.DeleteAsync(id, userid!, cancellationToken);

            return result.Success ? Ok(result) : NotFound(result);

        }







    }
}
