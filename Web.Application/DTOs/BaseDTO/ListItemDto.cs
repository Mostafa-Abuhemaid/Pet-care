using System;
using System.Collections.Generic;

namespace Web.Application.DTOs.BaseDTO
{
    public class ListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string LogoUrl { get; set; }
        public decimal PricePerNight { get; set; }
        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }
        public string Location { get; set; }
        public List<string> Services { get; set; }
        public bool IsEmergencyAvailable { get; set; }
    }
}
