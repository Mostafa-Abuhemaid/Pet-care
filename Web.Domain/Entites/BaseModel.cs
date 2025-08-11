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

        public int Id { get; set; }
        public DateTime Createdon { get; set; } = DateTime.UtcNow;
        public bool Deleted { get; set; }
        public string? UpdatedByid { get; set; }
        public DateTime? Updatedon { get; set; }
    }
}
