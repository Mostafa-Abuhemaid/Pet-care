using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class BaseModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Createdon { get; set; } = DateTime.UtcNow;

        public bool Deleted { get; set; }

        [MaxLength(450)]
        public string? UpdatedByid { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Updatedon { get; set; }

       

        //public AppUser CreatedBy { get; set; } = default!;

       // public ApplicationUser? UpdatedBy { get; set; }


    }
}
