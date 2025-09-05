using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.CartDTO
{
    public record CartTotals(decimal Subtotal, decimal Discount)
    {
        public decimal Tax { get; init; } = 20;
        public decimal FinalTotal => Subtotal + Tax - Discount;
    }
}
