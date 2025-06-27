using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.AccountDTO;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.Response;

namespace Web.Application.Interfaces
{
    public interface IPetProfileService
    {
        Task<BaseResponse<PetResponse>> AddAsync(PetRequest request ,string UserId,CancellationToken cancellationToken=default);
        Task<BaseResponse<PetResponse>>GetAsync(int id,string UserId,CancellationToken cancellationToken=default);
        Task<BaseResponse<IEnumerable< PetResponse>>>GetAllAsync(string UserId,CancellationToken cancellationToken=default);
        Task<BaseResponse<bool>> UpdateAsync(int id,PetRequest request, string UserId, CancellationToken cancellationToken = default);
        Task<BaseResponse<bool>> DeleteAsync(int id, string UserId, CancellationToken cancellationToken = default);
    }
}
