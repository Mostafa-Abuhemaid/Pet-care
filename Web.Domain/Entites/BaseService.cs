using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class BaseService:BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string logoUrl { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int AddressId { get; set; }
        public Address Address { get; set; } = default!;
        public decimal PricePerNight { get; set; }
    }
}
