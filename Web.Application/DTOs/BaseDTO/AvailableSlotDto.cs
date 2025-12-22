using System;

namespace Web.Application.DTOs.BaseDTO
{
    public class AvailableSlotDto
    {
        public DateOnly? Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AddressDto Address { get; set; }
        public decimal PricePerNight { get; set; }
        public bool IsBooked { get; set; }
    }
}
