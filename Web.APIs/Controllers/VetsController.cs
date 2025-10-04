using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Common;
using Web.Application.DTOs.VetDTO;
using Web.Application.Interfaces;
using Web.Domain.Enums;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VetsController(IVetService vetService) : ControllerBase
    {
        private readonly IVetService _vetService = vetService;

        [HttpPost("Add-New-Vet")]
        public async Task<IActionResult> Add([FromForm] VetRequest request)
        {
            var userid=User.GetUserId();
            var result=await _vetService.AddAsync(userid,request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Get-Vet/{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            var result=await _vetService.GetAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("GetAvailableSlots-Vet/{id}")]
        public async Task<IActionResult> GetAvailableSlots([FromRoute]int id,[FromBody]GetAvailableSlotsRequest request)
        {
            var result=await _vetService.GetAvailableSlotsAsync(id,request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Get-Reviews/{Vetid}")]
        public async Task<IActionResult> GetReviews([FromRoute]int Vetid)
        {
            var result = await _vetService.GetReviewsasync(Vetid);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Book-Vet/{VetclinicId}")]
        public async Task<IActionResult> BookingVet([FromRoute]int VetclinicId,[FromBody]BookVetDTO request)
        {
            var userid=User.GetUserId();
            var result=await _vetService.BookingVet(userid,VetclinicId,request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("ConfirmBooking-Vet/{VetclinicId}")]
        public async Task<IActionResult> ConfirmBooking([FromRoute] int VetclinicId)
        {
            var result = await _vetService.ConfirmBookingAsync(VetclinicId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Get-All-Vets")]
        public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters = default!, [FromQuery] AddtionalRequestFilters addtionalRequestFilters=default!)
        {
            var result=await _vetService.GetAllAsync(filters,addtionalRequestFilters);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-Vet/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] VetRequest request)
        {
            var result = await _vetService.UpdateAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("Delete-Vet/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result=await _vetService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
