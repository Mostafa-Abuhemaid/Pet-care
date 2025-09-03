using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.CartDTO
{
    public record PromoCodeResponse
    (
        int id,
        string Code ,
        decimal DiscountValue ,
        int MaxUsageCount ,
        int UsedCount ,
        DateTime StartDate ,
        DateTime EndDate ,
        bool IsActive 
        
        
     );
}
