using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class VetBookingService
    {
        public int VetBookingId { get; set; }
        public VetBooking VetBooking { get; set; } = default!;

        public int VetClinicServiceId { get; set; }
        public VetClinicService VetClinicService { get; set; } = default!;
    }
}
