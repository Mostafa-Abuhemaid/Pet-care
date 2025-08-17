using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.ProductDTO
{
    public record OffersProductResponse(
        int id,
        string name,
        string Discount,
        string ImageUrl
    );

}
