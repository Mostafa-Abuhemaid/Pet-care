using Azure.Core;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Api.Entities;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Web.Application.Common;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.Response;
using Web.Domain.Enums;
using Web.Infrastructure.Service;

namespace PetCare.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public  class PetController : ControllerBase 
        
    {
      //  private  BasePetService<T> _service;
        private readonly PetServiceFactory _factory;

        public PetController(PetServiceFactory factory)
        {
            _factory = factory;
        }
       

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PetRequest request, CancellationToken cancellationToken)
        {

            var _service = _factory.Create(request.petType);

            var result = await _service.AddAsync(request,User.GetUserId(), cancellationToken);
            return StatusCode(result.Success ? 200 : 400, result);
        }

        [HttpGet("Pets")]
        public async Task<IActionResult> GetAllBtType([FromQuery] PetType petType, CancellationToken cancellationToken)
        {
            var _service = _factory.Create(petType);

            var result = await _service.GetAllAsync(User.GetUserId(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] PetType petType, CancellationToken cancellationToken)
        {
            var _service = _factory.Create(petType);

            var result = await _service.GetAsync(id, User.GetUserId(), cancellationToken);
            return StatusCode(result.Success ? 200 : 404, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] PetRequest request, CancellationToken cancellationToken)
        {
            var _service = _factory.Create(request.petType);

            var result = await _service.UpdateAsync(id, request, User.GetUserId(), cancellationToken);
            return StatusCode(result.Success ? 200 : 404, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] PetType petType, CancellationToken cancellationToken)
        {
            var _service = _factory.Create(petType);

            var result = await _service.DeleteAsync(id, User.GetUserId(), cancellationToken);
            return StatusCode(result.Success ? 200 : 404, result);
        }

        [HttpGet("Avaliable_Pets_Mating")]
        public async Task<IActionResult> AvaliableMating([FromQuery]RequestFilters filters,CancellationToken cancellationToken)
        {
            var _service = _factory.Create(0);

            var result = await _service.AvaliableMatingAsync(filters, cancellationToken);
            return StatusCode(result.Success ? 200 : 404, result);
        }
    }
}
