using System;
using System.Collections.Generic;
using Web.Application.DTOs.Reviews;

namespace Web.Application.DTOs.BaseDTO
{
    public class ServiceDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string LogoUrl { get; set; }
        public string Type { get; set; }
        public List<string> Services { get; set; }
        public string Location { get; set; }
        public decimal PricePerNight { get; set; }
        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }
       public  List<VetUserReviewDto>? Reviews { get; set; }
    }
}
