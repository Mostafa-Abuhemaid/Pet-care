using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Web.Domain.Entites;

namespace PetCare.Api.Entities
{
    [NotMapped] // Explicitly mark as not mapped to database

    public abstract class Pet : BaseModel
    {
       
            [Required]
            [MaxLength(100)]
            public string Name { get; set; }

            [Required]
            [MaxLength(100)]
            public string Breed { get; set; }

            [Required]
            [Range(0, 50)]
            public int Age { get; set; }

            [Required]
            [MaxLength(10)]
            public string Gender { get; set; }

            [Url]
            [MaxLength(255)]
            public string PhotoUrl { get; set; }

            [Required]
            [ForeignKey("AppUser")]
            public string AppUserId { get; set; }

            public AppUser AppUser { get; set; }

            public bool IsInBreedingPeriod { get; set; }
        }

}
