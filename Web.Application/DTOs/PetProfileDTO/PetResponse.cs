using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Enums;

namespace Web.Application.DTOs.PetProfileDTO
{
    public record PetResponse(
    int Id,
    string Name,
    string Breed,
    DateOnly BirthDay,
    string Color,
    Double Weight,
    Double height,
    string Characteristic,
    string Gender,
    string MedicalConditions,
    string ?breedingRequestStatus,
    string PhotoUrl

     );
}
