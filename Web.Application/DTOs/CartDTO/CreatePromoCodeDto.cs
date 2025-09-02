using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.CartDTO
{
    public record CreatePromoCodeDto
    (
        decimal DiscountValue 
       , DateTime StartDate 
      , DateTime EndDate 
      , int MaxUsageCount  
        );
   
}
