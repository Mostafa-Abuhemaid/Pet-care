using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.VetDTO
{
    public record VetDetailsDto(
        int Id,
        string Name,
        string Description,
        string Phone,
        string logoUrl,
        string Type,               
        List<string> Services,    
        string Location,           
        decimal PricePerNight,
        bool IsEmergencyAvailable,
        int Experience,            // Years
        int CountOfPatients,
        double AverageRating,
        int ReviewsCount,
    List<VetUserReviewDto>? Reviews,
    List<VetScheduleDto>? Schedules
    );

}
