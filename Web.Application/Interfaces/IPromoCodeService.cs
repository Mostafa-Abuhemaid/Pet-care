using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.CartDTO;
using Web.Application.Response;
using Web.Domain.Entites;


namespace Web.Application.Interfaces
{
    public interface IPromoCodeService
    {
        Task<BaseResponse<decimal>> ApplyPromoCode(CartResponse response, string code);
        Task<BaseResponse<PromoCodeResponse>> CreatePromoCodeAsync(CreatePromoCodeDto dto);
        Task<BaseResponse<IEnumerable<PromoCodeResponse>>> GetAllPromoCodeAsync();
        Task<BaseResponse<bool>> DeletePromoCodeAsync(int codeid);
        Task<BaseResponse<bool>> ToggelStatusActive(int codeid);
    }
}
