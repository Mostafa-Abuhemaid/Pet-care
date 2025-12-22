using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.Reviews
{
    public record VetUserReviewDto
    (
   string Userid ,
   string UserName,
   string Comment ,
   DateTime DatePosted 
);



}
