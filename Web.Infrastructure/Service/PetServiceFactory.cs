using FluentValidation;
using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.PetProfileDTO;
using Web.Domain.Enums;
using Web.Infrastructure.Data;

namespace Web.Infrastructure.Service
{
    public class PetServiceFactory
    {
        private readonly AppDbContext _context;
        private readonly IValidator<PetRequest> _validator;
        public PetServiceFactory(AppDbContext context, IValidator<PetRequest> validator)
        {
            _context = context;
            _validator = validator;
        }
        public IBasePetService Create(PetType petType)
        {
            

        
            return petType switch
            {
                PetType.cat => new BasePetService<Pet_Cat>(_context, _validator),
                PetType.dog => new BasePetService<Pet_Dog>(_context, _validator),
                _ => throw new NotImplementedException()
            };
        }
    }
}
