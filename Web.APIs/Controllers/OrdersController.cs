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

        [HttpPost("Confirm")]
        public async Task<IActionResult> ConfirmOrder([FromBody] CreateOrderDto request)
        {
            var userId = User.GetUserId();
            var result = await _orderService.CreateOrderAsync(userId, request);
            return result.Success ? Ok(result) : BadRequest(result);
            //TODOO // ممكن تغير حالة الأوردر أو تبعث إشعار للعميل
        }

    }
}
