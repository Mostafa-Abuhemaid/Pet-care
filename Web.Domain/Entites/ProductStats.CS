using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class ProductStats:BaseModel
    {
        public int ProductId { get; set; }
        public int SalesCount { get; set; }
        public Product Product { get; set; }
    }

}
