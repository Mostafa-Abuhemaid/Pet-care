using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.PetProfileDTO
{
    public record YourPetsDTO
        (
        string PhotoURL,
        string Name,
        string Breed,
        string Gender       
        );
}
