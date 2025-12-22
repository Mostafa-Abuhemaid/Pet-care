using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.Common;
using Web.Application.DTOs.BaseDTO;
using Web.Application.DTOs.Reviews;
using Web.Application.DTOs.VetDTO;
using Web.Application.Response;
using Web.Domain.Entites;

namespace Web.Application.Interfaces
{
    public interface IBoardingService
    {
        Task<BaseResponse<ServiceDetailsDTO>> AddAsync(string userId, AddRequest request);
        Task<BaseResponse<bool>> DeleteAsync(int id);
        Task<BaseResponse<PaginatedList<ListItemDto>>> GetAllAsync(RequestFilters filters = default!, AddtionalRequestFilters addtionalRequestFilters = default!);

        Task<BaseResponse<ServiceDetailsDTO>> GetAsync(int id);
        Task<BaseResponse<bool>> UpdateAsync(int id, AddRequest request);

        Task<BaseResponse<VetReviewsDto>> GetReviewsasync(int VetId);
    }
}
