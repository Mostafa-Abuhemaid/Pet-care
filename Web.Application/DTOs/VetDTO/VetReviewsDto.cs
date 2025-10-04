using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;

namespace Web.Application.DTOs.VetDTO
{
    public record VetReviewsDto
(
    int VetClinicId,
    double AverageRating,
    int CountRating,
    List<VetUserReviewDto> ReviewDtos
        
  
        );
}
