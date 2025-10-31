using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.AccountDTO;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.DTOs.ProductDTO;
using Web.Application.DTOs.VetDTO;
using Web.Application.Response;

namespace Web.Application.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<TokenDTO>> LoginAsync(LoginDTO loginDto);
        Task<BaseResponse<TokenDTO>> RegisterAsync(RegisterDTO registerDTO);
        Task<BaseResponse<string>> ForgotPasswordAsync(ForgetPasswordDto request);
        Task<BaseResponse<bool>> VerifyOTPAsync(VerfiyCodeDto verify);
        Task<BaseResponse<bool>> ResetPasswordAsync(ResetPasswordDto resetPassword);
        Task<BaseResponse<UserProfileDTO>> GetProfileUser(string userid);
        Task<BaseResponse<IEnumerable<UserHistoryDto>>> GetHistory(string userid);
        Task<BaseResponse<IEnumerable<ProductResponse>>> GetFavoriteProduct(string userid);
        Task<BaseResponse<IEnumerable<YourPetsDTO>>> GetFavoritePets(string userid);
        Task<BaseResponse<IEnumerable<VetListItemFavoriteDto>>> GetFavoriteVetClinc(string userid);
    }
}
