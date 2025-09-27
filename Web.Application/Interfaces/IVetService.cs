using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.Common;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.DTOs.VetDTO;
using Web.Application.Response;

namespace Web.Application.Interfaces
{
    public interface IVetService
    {
        Task<BaseResponse<VetDetailsDto>> AddAsync(string userId, VetRequest request);
        Task<BaseResponse<PaginatedList<VetListItemDto>>> GetAllAsync();
        Task<BaseResponse<VetDetailsDto>> GetAsync(int id);
        Task<BaseResponse<bool>> UpdateAsync(int id, VetRequest request);
        Task<BaseResponse<bool>> DeleteAsync(int id);
    }
}
