using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Common;
using Web.Application.DTOs.BaseDTO;
using Web.Application.DTOs.VetDTO;
using Web.Application.Interfaces;
using Web.Domain.Enums;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BoardingController(IBoardingService boardingService) : ControllerBase
    {
            private readonly IBoardingService _boardingService = boardingService;

            [HttpPost("Add-New-Boarding")]
            public async Task<IActionResult> Add([FromForm] AddRequest request)
            {
                var userid = User.GetUserId();
                var result = await _boardingService.AddAsync(userid, request);
                return result.Success ? Ok(result) : BadRequest(result);
            }

            [HttpGet("Get-Boarding/{id}")]
            public async Task<IActionResult> Get([FromRoute] int id)
            {
                var result = await _boardingService.GetAsync(id);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            //[HttpPost("GetAvailableSlots-Boarding/{id}")]
            //public async Task<IActionResult> GetAvailableSlots([FromRoute] int id, [FromBody] GetAvailableSlotsRequest request)
            //{
            //    var result = await _boardingService.GetAvailableSlotsAsync(id, request);
            //    return result.Success ? Ok(result) : BadRequest(result);
            //}

            [HttpGet("Get-Reviews/{id}")]
            public async Task<IActionResult> GetReviews([FromRoute] int id)
            {
                var result = await _boardingService.GetReviewsasync(id);
                return result.Success ? Ok(result) : BadRequest(result);
            }

            //[HttpPost("Book-Boarding/{id}")]
            //public async Task<IActionResult> BookingVet([FromRoute] int id, [FromBody] BookVetDTO request)
            //{
            //    var userid = User.GetUserId();
            //    var result = await _boardingService.BookingVet(userid, VetclinicId, request);
            //    return result.Success ? Ok(result) : BadRequest(result);
            //}

            //[HttpGet("ConfirmBooking-Vet/{VetclinicId}")]
            //public async Task<IActionResult> ConfirmBooking([FromRoute] int VetclinicId)
            //{
            //    var userid = User.GetUserId();
            //    var result = await _vetService.ConfirmBookingAsync(userid, VetclinicId);
            //    return result.Success ? Ok(result) : BadRequest(result);
            //}

            //[HttpGet("booking/{bookingId}/download")]
            //public async Task<IActionResult> DownloadByBookingId(int bookingId)
            //{
            //    var result = await _vetService.GenerateVetReceiptPdfByBookingIdAsync(bookingId);
            //    return File(result.Data.Content, result.Data.ContentType, result.Data.FileName);
            //}

            [HttpGet("Get-All-Boardings")]
            public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters = default!, [FromQuery] AddtionalRequestFilters addtionalRequestFilters = default!)
            {
                var result = await _boardingService.GetAllAsync(filters, addtionalRequestFilters);
                return result.Success ? Ok(result) : BadRequest(result);
            }

            [HttpPut("update-Boarding/{id}")]
            public async Task<IActionResult> Update([FromRoute] int id, [FromForm] AddRequest request)
            {
                var result = await _boardingService.UpdateAsync(id, request);
                return result.Success ? Ok(result) : BadRequest(result);
            }

            [HttpDelete("Delete-Boarding/{id}")]
            public async Task<IActionResult> Delete([FromRoute] int id)
            {
                var result = await _boardingService.DeleteAsync(id);
                return result.Success ? Ok(result) : BadRequest(result);
            }

        }
    }
