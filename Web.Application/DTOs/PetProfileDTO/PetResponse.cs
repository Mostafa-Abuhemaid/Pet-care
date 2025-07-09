using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.PetProfileDTO
{
    public record PetResponse(
            int Id,
        string Name,
       string Breed,
        DateOnly BirthDay,
        string Color,
         Double Weight,
       string Gender,
        string MedicalConditions,
         bool IsInBreedingPeriod,
     string PhotoUrl

        );
}
