using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class VetClinicService : BaseModel
    {
        public int VetClinicId { get; set; }   // FK
        public VetClinic VetClinic { get; set; } = default!;

        public string Name { get; set; } = string.Empty;  // "Emergency", "Grooming", ...
    }

}
