using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.VetDTO
{
    public record AvailableSlotDto(
        DateOnly? Date,
        DateTime StartTime,
        DateTime EndTime,
        AddressDto Address,
        decimal PricePerNight,
        //int countBookedit,
        bool IsBooked
    );
}
