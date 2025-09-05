using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Enums;

namespace Web.Domain.Entites
{
    public class Order : BaseModel
    {
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = default!;

        public int AddressId { get; set; }
        public Address Address { get; set; } = default!;

        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

        public ICollection<CartItem> Items { get; set; } = [];
    }

}
