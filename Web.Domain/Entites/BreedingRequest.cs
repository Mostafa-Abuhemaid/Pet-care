using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Web.Domain.Entites;
using Web.Domain.Enums;

namespace PetCare.Api.Entities
{
    public class BreedingRequest : BaseModel
    {
        public int RequesterPetId { get; set; }
        public Pet RequesterPet { get; set; } = default!;

        public int TargetPetId { get; set; }
        public Pet TargetPet { get; set; } = default!;

        public BreedingRequestStatus Status { get; set; }

        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Message { get; set; } = string.Empty;
    }


}
