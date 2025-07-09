using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Enums;

namespace Web.Application.DTOs.PetProfileDTO
{
    public record PetRequest
    (
       string Name ,
       string Breed ,
       DateOnly BirthDay,
        string Color,
         double Weight,
       string Gender ,
        string MedicalConditions,
         string breedingRequestStatus,
             IFormFile? Photo ,
                  PetType petType

    );
}
