using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Web.Domain.Entites;

namespace PetCare.Api.Entities
{
    [Table("Pet_Dogs")] // Explicit table name

    public class Pet_Dog: Pet
    {
        // comment to until test Api PetProfile and generate data for Cat_data
        
        //// Foreign key to owner
        //[Required]
        //[ForeignKey("Cat_Data")]
        //public int Cat_DataId { get; set; }
        //public Cat_Data Cat_Data { get; set; }

    }

}
