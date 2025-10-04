using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.PetProfileDTO;

namespace Web.Application.DTOs.AccountDTO
{
    public record UserProfileDTO
   (
        string id,
        string Name,
        string Email,
        string PhotoURL,
        List<YourPetsDTO> YourPetsName
        );
}
