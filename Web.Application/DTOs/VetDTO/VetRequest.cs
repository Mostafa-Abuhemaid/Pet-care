using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.VetDTO
{
    public record VetRequest(
     string Name,
     string Description,
     string Phone,
     IFormFile? Photo,
     string Type,            
     List<string> Services,  
     AddressDto Address,          
     decimal PricePerNight,
     bool IsEmergencyAvailable,
     int Experience,
     int CountOfPatients
 );

}
