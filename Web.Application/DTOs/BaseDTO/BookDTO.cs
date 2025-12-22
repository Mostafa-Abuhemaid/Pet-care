using System;
using System.Collections.Generic;

namespace Web.Application.DTOs.BaseDTO
{
    public class BookDTO
    {
        public int PetId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public List<int> ServiceIds { get; set; }
    }
}
