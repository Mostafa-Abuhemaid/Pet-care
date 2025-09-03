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
       string Description,
       string Size,
        double rate,
        int Price,
        string ImageUrl

    );
}
