using System.ComponentModel.DataAnnotations;
using Web.Domain.Entites;

namespace PetCare.Api.Entities
{
    public class VetClinic: BaseModel
    {
            [Required]
            [MaxLength(100)]
            public string Name { get; set; }

            [Required]
            [MaxLength(100)]
            public string Specialty { get; set; }

            [Required]
            [MaxLength(200)]
            public string Location { get; set; }

            [Required]
            public bool IsEmergencyAvailable { get; set; }

            public ICollection<VetReview> Reviews { get; set; }
    }
}


