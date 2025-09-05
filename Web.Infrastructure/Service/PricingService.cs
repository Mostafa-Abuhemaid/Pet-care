using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.CartDTO;
using Web.Domain.Entites;

namespace Web.Infrastructure.Service
{
    public class PricingService
    {
        public CartTotals CalculateCartTotals(Cart cart, PromoCode? promo)
        {
            var subtotal = cart.Items.Sum(x => x.Product.Price * x.Quantity);
            var discount = promo?.DiscountValue ?? 0;
            var tax = 20;

            return new CartTotals(subtotal, discount) { Tax = tax };
        }
    }
}
