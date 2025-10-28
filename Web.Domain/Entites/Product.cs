using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class Product:BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Size {  get; set; }
        public double rate {  get; set; }
        public double StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
        public ProductStats ProductStats { get; set; } = default!;

        public ICollection<History> Histories { get; set; } = [];

    }
}
