using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.DataSeederDTO
{
    public class ProductJsonDto
    {
        public string name { get; set; } = string.Empty;
        public string price { get; set; } = string.Empty;  
        public string image_url { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public object brand { get; set; }
        public string category { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
    }

}
