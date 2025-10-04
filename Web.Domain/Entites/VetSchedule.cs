using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Enums;

namespace Web.Domain.Entites
{
    public class VetSchedule : BaseModel
    {
        public int VetClinicId { get; set; }
        public VetClinic vetClinic { get; set; } = default!;
        public DayOfWeekEnum DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

}
