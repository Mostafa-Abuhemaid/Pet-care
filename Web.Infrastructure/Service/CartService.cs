using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Web.Application.Common;
using Web.Application.Common.Constants;
using Web.Application.DTOs.CartDTO;
using Web.Application.DTOs.ProductDTO;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Domain.Entites;
using Web.Infrastructure.Persistence.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.Infrastructure.Service
{
    public class CartService(AppDbContext context) : ICartService
    {
        private readonly AppDbContext _context = context;

        public async Task<BaseResponse<bool>> AddToCartAsync(CartItemRequest request, string userId)
        {

            if (await _context.Products.Where(p => p.Id == request.ProductId && !p.Deleted).FirstOrDefaultAsync() is not { } product)
                return new BaseResponse<bool>(false, "Error! Product not found");

            if (await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId) is not { } cart)
            {
                cart = new Cart { UserId = userId, Updatedon = DateTime.UtcNow };
                _context.Carts.Add(cart);
            }

            var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductId == request.ProductId);

            if (cartItem is not null)
            {
                if (cartItem.Quantity + request.Quantity > product.StockQuantity)
                    return new BaseResponse<bool>(false, "Error! Quantity exceeds available stock");

                cartItem.Quantity += request.Quantity;
            }
            else
            {
                if (request.Quantity > product.StockQuantity)
                    return new BaseResponse<bool>(false, "Error! Not enough stock");

                cart.Items.Add(new CartItem
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                });
            }

            cart.Updatedon = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new BaseResponse<bool>(true, "Succeeded");
        }

        public async Task<BaseResponse<CartResponse>> GetCartAsync(string UserId)
        {
           var query=await _context.Carts.Include(c=>c.Items).ThenInclude(c=>c.Product).ThenInclude(p=>p.Category)
                .FirstOrDefaultAsync(x=>x.UserId == UserId);

            if(query is  null)
                return new BaseResponse<CartResponse>(false, "The requested Cart was not found");

            var items = query.Items.Select(i => new CartItemResponse(
                i.Product!.Id,
                i.Product.Name,
                i.Product.Category.Name,
                i.Product.Size,
                i.Product.ImageUrl,
                i.Product.Price,
                i.Quantity

           ));

            var response = new CartResponse
                  (
                query.Id,
                UserId,
                items,
            query.TotalAmount
                );

            return new BaseResponse<CartResponse>(true, "Cart items retrieved successfully", response);

        }



    }

    }
