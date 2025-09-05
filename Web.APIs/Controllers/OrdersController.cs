using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Application.DTOs.OrderDTO;
using Web.Application.Interfaces;
using Web.Domain.Enums;
using Web.Infrastructure.Service;

namespace Web.APIs.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        [HttpPost("create/cash-on-delivery")]
        public async Task<IActionResult> CreateCashOnDeliveryOrder([FromBody] CreateOrderDto request)
        {
            request.PaymentMethod = PaymentMethod.CashOnDelivery;
            var userId = User.GetUserId();
            var result = await _orderService.CreateOrderAsync(userId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("create/credit-card")]
        public async Task<IActionResult> CreateCreditCardOrder([FromBody] CreateCardOrderDto request)
        {
            request.PaymentMethod = PaymentMethod.CreditCard;
            var userId = User.GetUserId();
            var result = await _orderService.CreateOrderAsync(userId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("create/wallet")]
        public async Task<IActionResult> CreateWalletOrder([FromBody] CreateWalletOrderDto request)
        {
            request.PaymentMethod = PaymentMethod.Wallet;
            var userId = User.GetUserId();
            var result = await _orderService.CreateOrderAsync(userId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
