using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.NotificationDTO
{
    public record NotificationResponseDTO
        (
        string IconURL,
          int id,
         string Message,
         DateTime CreatedAt,
         bool IsRead,
         int PetId
        );
}
