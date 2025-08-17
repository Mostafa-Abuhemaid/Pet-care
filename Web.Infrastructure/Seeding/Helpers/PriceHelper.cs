using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Infrastructure.Seeding.Helpers
{
    public static class PriceHelper
    {
        public static int ParsePrice(string priceString)
        {
            if (string.IsNullOrWhiteSpace(priceString))
                return 0;

            var cleaned = new string(priceString.Where(char.IsDigit).ToArray());

            return int.TryParse(cleaned, out var price) ? price : 0;
        }


        public static string Parsebrand(int priceString)
        {
            var brand=priceString.ToString();
            return brand;
        }
    }
}
