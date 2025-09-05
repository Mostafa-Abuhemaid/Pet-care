using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;

namespace Web.Application.DTOs.CartDTO
{
    public record CartItemResponse
    (
        int ProductId,
        string Name,
        string categoryname,
        string size,
        string ImageUrl,
        decimal Price,
        int Quantity

    );
}
