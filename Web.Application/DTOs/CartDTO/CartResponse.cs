using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.CartDTO
{
    public record CartResponse(
      int CartId,
      string UserId,
      IEnumerable<CartItemResponse> Items,
      decimal OrderAmount
  )
    {
        public decimal Tax { get; init; } = 20;
        public decimal TotalPayment => OrderAmount + Tax;
    }



}
