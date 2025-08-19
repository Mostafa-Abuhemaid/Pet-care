using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class Special_Offers : BaseModel
    {
      
        public string ImgURL { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
    }
}
