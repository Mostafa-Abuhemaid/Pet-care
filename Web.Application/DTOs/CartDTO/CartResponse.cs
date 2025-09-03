using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.CartDTO
{
    public record CartResponse(
      int CartId,
      string UserId,
      IEnumerable<CartItemResponse> Items,
      double OrderAmount
  )
    {
        public double Tax { get; init; } = 20;
        public double TotalPayment => OrderAmount + Tax;
    }

    public record CartTotals(double Subtotal, double Discount)
    {
        public double Tax { get; init; } = 20;
        public double FinalTotal => Subtotal + Tax - Discount;
    }

}
