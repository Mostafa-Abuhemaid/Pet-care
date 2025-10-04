using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.VetDTO
{
    public record VetListItemDto(
        int Id,
        string Name,
         string Type,
        string logoUrl,
        decimal PricePerNight,
        double AverageRating,
        int ReviewsCount,
        string Location,   // Ex: "Egypt / Cairo / Moqattam"
        List<string> Services,  // ["Emergency", "Grooming"]
        bool IsEmergencyAvailable
    );

    public record VetListItemFavoriteDto(
    int Id,
    string Name,
     string Type,
    string logoUrl,
    decimal PricePerNight
    );

}
