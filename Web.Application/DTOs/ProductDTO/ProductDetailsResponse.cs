using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.ProductDTO
{
    public record ProductDetailsResponse(
        int Id,
        string Name,
        string Description,
        string ImageUrl,
        int Price,
        string CategoryName
    );
}
