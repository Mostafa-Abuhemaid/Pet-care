using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.AccountDTO
{
    public record UserHistoryDto
(
        string UserId,
        string Name,
        string Desciption,
        string Unit ,
        string imageURl ,
        decimal Price , 
        DateTime Date 
        );
}
