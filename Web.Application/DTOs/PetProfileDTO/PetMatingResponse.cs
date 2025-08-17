using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.PetProfileDTO
{
    public record PetMatingResponse(
        int Id,
        string Name,
        string Gender,
        string Breed,
        string Color,
        string PhotoUrl,
        string Type   
    );

}
