using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;
using Web.Domain.Entites;
using Web.Domain.Enums;
using static Azure.Core.HttpHeader;

namespace PetCare.Api.Entities
{
    public class Cat_Data : Data
    {
        [Required]
        [MaxLength(100)]
        public string Breed { get; set; }
        [Required]
        public double MaleAverageWeight { get; set; }
        [Required]

        public double MaleAverageSize { get; set; }
        [Required]
        public string MaleTemperament { get; set; }
        [Required]
        public double FemaleAverageWeight { get; set; }
        [Required]
        public double FemaleAverageSize { get; set; }
        [Required]
        public string FemaleTemperament { get; set; }


        [MaxLength(500)]
        public string Physical_Characteristics { get; set; }

        public Energy_Level Energy_Level { get; set; }

        [MaxLength(200)]
        public string Good_With { get; set; }

        public Trainability Trainability { get; set; }

        public Vocalization vocalization { get; set; }

        [MaxLength(500)]
        public string Health_Risks { get; set; }

        [MaxLength(500)]
        public string Common_Vaccinations { get; set; }

        [MaxLength(200)]
        public string Vaccination_Frequency_ofTiming { get; set; }

        [MaxLength(500)]
        public string Common_Diseases_Prevention { get; set; }

        [MaxLength(100)]
        public string Grooming_Frequency { get; set; }

        public Shedding_Level Shedding_Level { get; set; }

        [Required]
        public bool Hypoallergenic { get; set; }



    }

}
