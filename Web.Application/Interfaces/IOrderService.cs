using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.OrderDTO;
using Web.Application.Response;
using Web.Domain.Entites;

namespace Web.Application.Interfaces
{
    public interface IOrderService
    {
        Task<BaseResponse<bool>> CreateOrderAsync(string UserId, CreateOrderDto dto);
    }
}
