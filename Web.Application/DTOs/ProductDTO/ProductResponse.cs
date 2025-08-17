using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.ProductDTO
{
    public record ProductResponse
    (
        int id,
        string name,
        double StockQuantity,
        int Price,
        string ImageUrl
    );
}
