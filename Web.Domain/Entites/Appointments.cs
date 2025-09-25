using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.Domain.Entites
{
    public class Appointments : BaseModel
    {
        public int VetClinicId { get; set; }
        public VetClinic VetClinic { get; set; } = default!;
        public string AppUserId { get; set; } = string.Empty;
        public AppUser AppUser { get; set; } = default!;
        public DateOnly Date { get;set; }            // التاريخ اللي اتحدد
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = string.Empty;//(Booked / Cancelled / Completed)
    }
}
