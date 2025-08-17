using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.Common;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.Response;

namespace Web.Application.Interfaces
{
    public interface IBasePetService
    {
        public  Task<BaseResponse<PetResponse>> AddAsync(PetRequest request, string userId, CancellationToken cancellationToken = default);
        public Task<BaseResponse<IEnumerable<PetResponse>>> GetAllAsync(string userId, CancellationToken cancellationToken = default);

        public Task<BaseResponse<PetResponse>> GetAsync(int id, string userId, CancellationToken cancellationToken = default);
        public Task<BaseResponse<bool>> UpdateAsync(int id, PetRequest request, string userId, CancellationToken cancellationToken = default);
        public Task<BaseResponse<bool>> DeleteAsync(int id, string userId, CancellationToken cancellationToken = default);
        public Task<BaseResponse<PaginatedList<PetMatingResponse>>>AvaliableMatingAsync(RequestFilters filters,CancellationToken cancellationToken = default);



    }
}
