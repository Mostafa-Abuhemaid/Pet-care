using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class History:BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Desciption { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public string UserId { get; set; }=default!;
        public AppUser User { get; set; } = default!;
        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        public int? VetClinicId { get; set; }
        public VetClinic? VetClinic { get; set; }
    }
}
