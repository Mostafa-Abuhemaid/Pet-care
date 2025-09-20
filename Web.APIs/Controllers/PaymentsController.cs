using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Interfaces;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IPaymentService paymentService) : ControllerBase
    {
        private readonly IPaymentService _paymentService = paymentService;

        [HttpPost("create-intent/{orderid}")]
        public async Task<IActionResult> CreateIntent([FromRoute] int orderid)
        {
            var clientSecret = await _paymentService.CreatePaymentIntentAsync(orderid);
            return Ok( clientSecret );
        }

    }
}