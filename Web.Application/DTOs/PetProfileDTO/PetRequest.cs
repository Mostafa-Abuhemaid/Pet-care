using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.PetProfileDTO
{
    public record PetRequest
    (
       string Name ,
       string Breed ,
       int Age ,
       string Gender ,
       IFormFile Photo 
    );
}
