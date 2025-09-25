using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Web.Domain.Entites;

namespace PetCare.Api.Entities
{
    public class VetReview: BaseModel
    {
        public int VetClinicId { get; set; }
        public VetClinic VetClinic { get; set; } = default!;
        public string AppUserId { get; set; }=string.Empty;
        public AppUser AppUser { get; set; }= default!;
        public int Rating { get; set; }
        public string Comment { get; set; }= string.Empty;
        public DateTime DatePosted { get; set; }
    }
}


