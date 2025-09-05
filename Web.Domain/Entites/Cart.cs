using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class Cart:BaseModel
    {
        public string UserId { get; set; } =string.Empty;
        public AppUser User { get; set; } = default!;
        public ICollection<CartItem> Items { get; set; } = [];
        // FK
        public int? PromoCodeId { get; set; }
        public PromoCode? PromoCode { get; set; }
        public decimal TotalAmount => CalculateTotal();

        private decimal CalculateTotal()
        {
            return Items.Sum(item => item.Product.Price * item.Quantity);
        }
    }
}
