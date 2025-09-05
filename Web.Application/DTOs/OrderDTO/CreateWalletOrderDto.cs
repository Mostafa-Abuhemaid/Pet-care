using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.OrderDTO
{
    public class CreateWalletOrderDto : CreateOrderDto
    {
        public string WalletPhone { get; set; } = string.Empty;
    }

}
