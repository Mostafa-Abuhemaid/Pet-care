using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Web.Application.DTOs.BaseDTO
{
    public class AddRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public IFormFile? Photo { get; set; }
        public string Type { get; set; }
        public List<string> Services { get; set; }
        public AddressDto Address { get; set; }
        public decimal PricePerNight { get; set; }
    }
}
