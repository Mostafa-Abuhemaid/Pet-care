using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Web.Domain.Entites;
using Web.Domain.Enums;

namespace PetCare.Api.Entities
{
    public class BreedingRequest : BaseModel
    {
        [Required]
        [ForeignKey("RequesterPet")]
        public int RequesterPetId { get; set; }

        public Pet RequesterPet { get; set; }

        [Required]
        [ForeignKey("TargetPet")]
        public int TargetPetId { get; set; }

        public Pet TargetPet { get; set; }

       
        public BreedingRequestStatus Status { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime RequestDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [MaxLength(500)]
        public string Message { get; set; }
    }


}
