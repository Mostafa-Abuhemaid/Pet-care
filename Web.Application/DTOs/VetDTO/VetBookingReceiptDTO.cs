using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.VetDTO
{
    public record VetBookingReceiptDTO
(
     string ReceiptNumber,
    int BookingId,
    string PetName,
    string ClinicName,
    DateTime Date,
    TimeSpan Time,
    decimal Price,
    List<string> Services,
    string Location,
    string InstructionsOrTerms 


        );
}
