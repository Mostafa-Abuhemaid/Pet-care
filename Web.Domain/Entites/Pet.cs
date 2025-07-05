using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Web.Domain.Entites;
using Web.Domain.Enums;

namespace PetCare.Api.Entities
{
    [NotMapped] // Explicitly mark as not mapped to database

    public abstract class Pet : BaseModel
    {
        public Pet() { }
       
            [Required]
            [MaxLength(100)]
            public string Name { get; set; } = string.Empty;

        [Required]
            [MaxLength(100)]
            public string Breed { get; set; } = string.Empty;

        //[Required]
        //public DateOnly BirthDay { get; set; }

        [Required]
            [MaxLength(10)]
            public string Gender { get; set; } = string.Empty;

            [Required]
            [MaxLength(10)]
            public string Color { get; set; }=string.Empty;

        [Required]
        public int Weight { get; set; }
        [Required]
        [MaxLength(1000)]
        public string MedicalCondidtions { get; set; } = string.Empty;

             [Url]
            [MaxLength(255)]
            public string PhotoUrl { get; set; } = string.Empty;

        [Required]
            [ForeignKey("AppUser")]
            public string AppUserId { get; set; } = string.Empty;
        [Required]
        public PetType petType { get; set; }
        public AppUser AppUser { get; set; } = default!;

            public bool IsInBreedingPeriod { get; set; }
        }

}
