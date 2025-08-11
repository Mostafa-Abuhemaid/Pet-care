using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class Category:BaseModel
    {
        public string Name { get; set; }=string.Empty;
        public string Description { get; set; }=string.Empty;
        public string ImageUrl {  get; set; }=string.Empty;
       public ICollection<Product> Products { get; set; } = [];
    }
}
