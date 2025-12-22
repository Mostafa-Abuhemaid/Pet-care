using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class Boarding: BaseService
    {
        public ICollection<Pet> pets { get; set; } = [];

        public ICollection<VetClinicService> Services { get; set; } = [];
        public ICollection<VetReview> Reviews { get; set; } = [];
        public ICollection<Appointments> appointments { get; set; } = [];
        public ICollection<History> Histories { get; set; } = [];

    }
}
