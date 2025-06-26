using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Web.Domain.Entites;

namespace PetCare.Api.Entities
{
    public class VetReview: BaseModel
    {
        [Required]
        [ForeignKey("VetClinic")]
        public int VetClinicId { get; set; }

        public VetClinic VetClinic { get; set; }

        [Required]
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DatePosted { get; set; }
    }
}


