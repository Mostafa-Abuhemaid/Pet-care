using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Common;
using Web.Application.Response;
using Web.Infrastructure.Service;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController(PetServiceFactory factory) : ControllerBase
    {
        private readonly PetServiceFactory _factory = factory;

        [HttpGet("Avaliable_Pets_Mating")]
        public async Task<IActionResult> AvaliableMating([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
        {
            var _service = _factory.Create(0);

            var result = await _service.AvaliableMatingAsync(filters, cancellationToken);
            return result.Success? Ok(result):NotFound();
        }
    }
}
