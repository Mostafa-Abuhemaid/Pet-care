
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Web.Application.Common;
using Web.Application.Common.Constants;
using Web.Application.DTOs.ProductDTO;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Domain.Entites;
using Web.Infrastructure.Persistence.Data;

namespace Web.Infrastructure.Service
{
    public class ProductService(AppDbContext context,ICacheService cacheService) : IProductService
    {
        private readonly AppDbContext _context = context;
        private readonly ICacheService _cacheService = cacheService;
        private const string _cachePrefix = "availableProducts";
        public async Task<BaseResponse<PaginatedList<ProductResponse>>> GetAllAsync(RequestFilters filters = default!, int? CategoryId = null)
        {
            if (!await _context.categories.AnyAsync(c => c.Id == CategoryId||CategoryId==null))
                return new BaseResponse<PaginatedList<ProductResponse>>(false, ProductMessages.CategoryNotFound);

            var cacheKey = $"{_cachePrefix}-AllProducts-{CategoryId}";
            var Cachedvalue = await _cacheService.GetAsync<List<ProductResponse>>(cacheKey);

            PaginatedList<ProductResponse> products =default;
            if (Cachedvalue is null)
            {
                var query = _context.Products.AsQueryable();

                if (CategoryId is not null)
                    query = query.Where(p => p.CategoryId == CategoryId);

                if (filters != null)
                {
                    if (!string.IsNullOrEmpty(filters.SearchValue))
                        query = query.Where(p => p.Name.Contains(filters.SearchValue));

                    var (sortColumn, sortDirection) = GetValidatedSortOptions(filters);
                    query = query.OrderBy($"{sortColumn} {sortDirection}");

                }
                var source = query
                    .ProjectToType<ProductResponse>()
                    .AsNoTracking();

                 products = await PaginatedList<ProductResponse>
                    .CreateAsync(source, filters!.PageNumber, filters.PageSize);

                await _cacheService.SetAsync(cacheKey, products.Items);


            }
            else
            {
                products = new PaginatedList<ProductResponse>(
                    Cachedvalue, Cachedvalue.Count, filters.PageNumber, filters.PageSize);
            }

            return new BaseResponse<PaginatedList<ProductResponse>>(true, ProductMessages.ProductsRetrieved, products);
        }
        public async Task<BaseResponse<PaginatedList<ProductResponse>>> GetBestSellerProductsAsync(RequestFilters filters = null)
        {
            var cacheKey = $"{_cachePrefix}-BestSellersProducts";
            var Cachedvalue = await _cacheService.GetAsync<List<ProductResponse>>(cacheKey);

            PaginatedList<ProductResponse> products = default;
            if (Cachedvalue is null)
            {
                var query = _context.Products
                .Where(p => !p.Deleted && p.StockQuantity > 0)
                .OrderByDescending(p => p.ProductStats.SalesCount)
                .ThenByDescending(p => p.StockQuantity)
                .ThenBy(p => p.Price)
                .ProjectToType<ProductResponse>()
                .AsNoTracking();
                products = await PaginatedList<ProductResponse>
                        .CreateAsync(query, filters!.PageNumber, filters.PageSize);
                await _cacheService.SetAsync(cacheKey, products.Items);
            }
            else
            {
                products=new PaginatedList<ProductResponse>(Cachedvalue,Cachedvalue.Count, filters.PageNumber, filters.PageSize);
            }

            return new BaseResponse<PaginatedList<ProductResponse>>(true, ProductMessages.ProductsRetrieved, products);
        }
        public async Task<BaseResponse<List<OffersProductResponse>>> GetSpecialOffersAsync()
        {
            var items = await _context.Products
              .AsNoTracking()
              .Where(p => !p.Deleted && p.StockQuantity > 0)
              .OrderBy(p => p.Createdon)
              .Take(6)
              .Select(p => new { p.Id, p.Name, p.ImageUrl })
              .ToListAsync();

            var offers = items
                .Select((p, index) => new OffersProductResponse(
                    p.Id,
                    p.Name,
                    GetRandomDiscount(index),
                    p.ImageUrl))
                .ToList();

            return new BaseResponse<List<OffersProductResponse>>(
                true,
                ProductMessages.ProductsRetrieved,
                offers);
        }

        public async Task<BaseResponse<ProductDetailsResponse>>GetAsync(int id)
            {
            if(await _context.Products.Include(x=>x.CategoryId).FirstOrDefaultAsync(p=> p.Id == id) is not { } product)
                return new BaseResponse<ProductDetailsResponse> (false, ProductMessages.ProductNotFound);

            return new BaseResponse<ProductDetailsResponse>(true, ProductMessages.ProductsRetrieved, product.Adapt<ProductDetailsResponse>());
        }


        private (string SortColumn, string SortDirection) GetValidatedSortOptions(RequestFilters filters)
        {
            var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase){"Name", "Price", "CreatedDate"};

            var sortColumn =
                !string.IsNullOrWhiteSpace(filters?.SortColumn) &&
                allowedSortColumns.Contains(filters.SortColumn)
                    ? filters.SortColumn
                    : "Id";

            var sortDirection = string.Equals(filters?.SortDirection, "desc", StringComparison.OrdinalIgnoreCase)
                ? "desc"
                : "asc";

            return (sortColumn, sortDirection);
        }

        private string GetRandomDiscount(int index)
        {
            var discounts = new[] { "10% OFF", "15% OFF", "Buy 1 Get 1", "Save 50 EGP", "25% OFF", "Free Shipping" };
            return discounts[index % discounts.Length];
        }

    }
}
