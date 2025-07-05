using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.Response;
using PetCare.Api.Entities;
using Web.Infrastructure.Service;
using Web.Domain.Enums;
using FluentValidation;
using System.Text.RegularExpressions;
using Azure.Core;

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

        [HttpGet]
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
    }
}
