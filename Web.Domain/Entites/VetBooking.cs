using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Enums;

namespace Web.Domain.Entites
{

    public class VetBooking:BaseModel
    {
        public string UserId { get; set; } =string.Empty;
        public int PetId { get; set; }       
        public int VetClinicId { get; set; } 
        public DateTime Date { get; set; }  
        public TimeSpan Time { get; set; }
        public decimal Price { get; set; }
        public BookingStatus Status { get; set; }

        public string ReceiptNumber { get; set; } = string.Empty;
        public virtual AppUser User { get; set; } = default!;
        public virtual Pet Pet { get; set; } = default!;
        public virtual VetClinic VetClinic { get; set; } = default!;
        public ICollection<VetBookingService> VetBookingServices { get; set; } = [];
    }
}
