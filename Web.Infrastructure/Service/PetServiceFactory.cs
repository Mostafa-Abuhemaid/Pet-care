using FluentValidation;
using Microsoft.Extensions.Configuration;
using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.Interfaces;
using Web.Domain.Enums;
using Web.Infrastructure.Data;

namespace Web.Infrastructure.Service
{
    public class PetServiceFactory
    {
        private readonly AppDbContext _context;
        private readonly IValidator<PetRequest> _validator;
        private readonly IConfiguration _configuration;
        public PetServiceFactory(AppDbContext context, IValidator<PetRequest> validator, IConfiguration configuration)
        {
            _context = context;
            _validator = validator;
            _configuration = configuration;
        }
        public IBasePetService Create(PetType petType)
        {
            

        
            return petType switch
            {
                PetType.cat => new BasePetService<Pet_Cat>(_context, _validator, _configuration),
                PetType.dog => new BasePetService<Pet_Dog>(_context, _validator, _configuration),
                _ => throw new NotImplementedException()
            };
        }
    }
}
