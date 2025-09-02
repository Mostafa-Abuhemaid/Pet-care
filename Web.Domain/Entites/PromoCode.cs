using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class PromoCode
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public int MaxUsageCount { get; set; } = 0;
        public int UsedCount { get; set; } = 0;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
