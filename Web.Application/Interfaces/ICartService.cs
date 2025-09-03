using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.CartDTO;
using Web.Application.Response;

namespace Web.Application.Interfaces
{
    public interface ICartService
    {
        Task<BaseResponse<bool>> AddToCartAsync(CartItemRequest request,string UserId);
        Task<BaseResponse<CartResponse>> GetCartAsync(string UserId);
    }
}
