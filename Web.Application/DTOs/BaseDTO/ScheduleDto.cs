using System;

namespace Web.Application.DTOs.BaseDTO
{
    public class ScheduleDto
    {
        public string DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
