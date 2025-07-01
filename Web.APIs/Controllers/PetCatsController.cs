
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.Api.Controllers;
using PetCare.Api.Entities;
using Web.APIs.Controllers;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.Interfaces;
using Web.Infrastructure.Service;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PetCatsController : BasePetController<Pet_Cat>
{
    public PetCatsController(BasePetService<Pet_Cat> service, IValidator<PetRequest> validator)
        : base(service)
    {
    }
}

