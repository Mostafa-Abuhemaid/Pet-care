using System.ComponentModel.DataAnnotations;
using Web.Domain.Entites;

namespace PetCare.Api.Entities
{
    public class VetClinic: BaseModel
    {
            public string Name { get; set; }=string .Empty;
            public string Description { get; set; }= string.Empty;
            public string Phone { get; set; }= string.Empty;
            public string logoUrl { get; set; }= string.Empty;
            public string Type { get; set; }=string.Empty;
            public List<string> Services { get; set; } = []; //"Emergency", "Grooming", "Vaccines"
           public int AddressId { get; set; }   
        public Address Address { get; set; } = default!;

        public decimal PricePerNight { get; set; }
            public bool IsEmergencyAvailable { get; set; }
            public int Experience { get; set; }
            public int CountOfPatients { get; set; }
            public ICollection<VetReview> Reviews { get; set; } = [];
            public ICollection<VetSchedule> vetSchedules { get; set; } = [];
            public ICollection<Appointments> appointments { get; set; } = [];
    }
}


