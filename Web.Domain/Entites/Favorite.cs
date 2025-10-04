using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class Favorite
    {
        public string UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int PetId { get; set; }
        public int VetClinicId { get; set; }
        public AppUser User { get; set; } = default!;
        public Product Product { get; set; }=default!;
        public Pet Pet { get; set; } = default!;

        public VetClinic VetClinic { get; set; } = default!;


    }
}
