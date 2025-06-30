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
      int Age,
        string Color,
         int Weight,
       string Gender,
        string MedicalCondidtions,
         bool IsInBreedingPeriod,
     string PhotoUrl

        );
}
