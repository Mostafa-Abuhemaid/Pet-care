using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.Common;
using Web.Application.DTOs.ProductDTO;
using Web.Application.Response;

namespace Web.Application.Interfaces
{
    public interface IProductService
    {
        Task<BaseResponse<PaginatedList<ProductResponse>>> GetAllAsync(RequestFilters filters = default!, int? CategoryId = null);
        Task<BaseResponse<List<OffersProductResponse>>> GetSpecialOffersAsync();
        Task<BaseResponse<PaginatedList<ProductResponse>>> GetBestSellerProductsAsync(RequestFilters filters = null!);
        Task<BaseResponse<ProductDetailsResponse>> GetAsync(int id);
        Task<BaseResponse<List<ProductResponse>>>Search(string query);
    }
}
