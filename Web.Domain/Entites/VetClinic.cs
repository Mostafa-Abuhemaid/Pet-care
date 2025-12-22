using System.ComponentModel.DataAnnotations;
using Web.Domain.Entites;

namespace PetCare.Api.Entities
{
    public class VetClinic: BaseService
    {
            public bool IsEmergencyAvailable { get; set; }
            public int Experience { get; set; }
            public int CountOfPatients { get; set; }
            public ICollection<VetClinicService> Services { get; set; } = [];
            public ICollection<VetReview> Reviews { get; set; } = [];
            public ICollection<VetSchedule> vetSchedules { get; set; } = [];
            public ICollection<Appointments> appointments { get; set; } = [];
            public ICollection<History> Histories { get; set; } = [];
    }
}


