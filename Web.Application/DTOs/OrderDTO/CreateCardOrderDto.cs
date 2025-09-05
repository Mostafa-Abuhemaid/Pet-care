using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.OrderDTO
{
    public class CreateCardOrderDto : CreateOrderDto
    {
        public string CardNumber { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
    }
}
