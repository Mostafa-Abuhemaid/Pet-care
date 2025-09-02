using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.CategoryDTO;
using Web.Application.Response;
using Web.Domain.Entites;

namespace Web.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<BaseResponse<GetCategoryDTO>> CreateCategoryAsync(SendCategoryDTO categoryDTO);
        Task<BaseResponse<bool>> DeleteCategoryAsync(int categoryId);
        Task<BaseResponse<GetCategoryDTO>> UpdateCategoryAsync(int Id , SendCategoryDTO categoryDTO);
        Task<BaseResponse<List<GetCategoryDTO>>> GetAllCategory();
       
    }
}
