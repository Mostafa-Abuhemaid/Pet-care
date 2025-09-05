using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PetCare.Api.Entities;
using Web.Application.DTOs.CartDTO;
using Web.Application.Interfaces;
using Web.Domain.Enums;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController(ICartService cartService,IPromoCodeService promoCodeService) : ControllerBase
    {
        private readonly ICartService _cartService = cartService;
        private readonly IPromoCodeService _promoCodeService = promoCodeService;

        [HttpPost("")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemRequest request)
        {
            var userid=User.GetUserId();
            var result=await _cartService.AddToCartAsync(request,userid);

            return result.Success ?Ok(result): BadRequest(new { message = result.Message });
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var userid=User.GetUserId();
            var result=await _cartService.GetCartAsync( userid);

            return result.Success ?Ok(result): NotFound(new { message = result.Message });
        }


        [HttpPost("apply-promo")]
        public async Task<IActionResult> ApplyPromo([FromBody] ApplyPromoRequest request)
        {
            var userId = User.GetUserId();

            var cartResult = await _cartService.GetCartAsync(userId);
            if (!cartResult.Success || cartResult.Data is null)
                return NotFound(new { message = "Cart not found" });

            var promoResult = await _promoCodeService.ApplyPromoCode(cartResult.Data, request.Code);

            if (!promoResult.Success)
                return BadRequest(new { message = promoResult.Message });

            var updatedCart = cartResult.Data with
            {
                OrderAmount = promoResult.Data - cartResult.Data.Tax
            };

            return Ok(new
            {
                message = promoResult.Message,
                totalBefore = cartResult.Data.TotalPayment,
                discountApplied = cartResult.Data.TotalPayment - promoResult.Data,
                totalAfter = promoResult.Data,

                cart = updatedCart
            });
        }




    }
}
