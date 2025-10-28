using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.OrderDTO;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Domain.Entites;
using Web.Domain.Enums;
using Web.Infrastructure.Persistence.Data;

namespace Web.Infrastructure.Service
{
    public class OrderService(PricingService pricingService, AppDbContext context) : IOrderService
    {
        private readonly AppDbContext _context = context;
        private readonly PricingService _pricingService = pricingService;

        public async Task<BaseResponse<int>> CreateOrderAsync(string userId, CreateOrderDto dto)
        {
            var cart = await _context.Carts
                .Include(x => x.PromoCode)
                .Include(x => x.Items).ThenInclude(i => i.Product)
                .SingleOrDefaultAsync(x => x.UserId == userId);

            if (cart == null || !cart.Items.Any())
                return new BaseResponse<int>(false, "Cart is empty");

            var totals = _pricingService.CalculateCartTotals(cart, cart.PromoCode);

            var order = new Order
            {
                UserId = userId,
                Address = BuildAddress(dto, userId),
                PaymentMethod = dto.PaymentMethod,
                Status = OrderStatus.Pending, // initial
                Subtotal = totals.Subtotal,
                Tax = totals.Tax,
                Discount = totals.Discount,
                Total = totals.FinalTotal,
                Createdon = DateTime.UtcNow
            };

            await _context.orders.AddAsync(order);

            var products = cart.Items.Select(x => x.Product).ToList();
            foreach (var product in products)
            {
                var quantity = cart.Items.First(x => x.ProductId == product.Id).Quantity;

                var history = new History
                {
                    Name = product.Name,
                    Desciption = product.Description,
                    Price = product.Price,
                    Unit = $"{quantity} Peace",
                    Date = DateTime.Now,
                    UserId = userId,
                    ProductId = product.Id
                };
                _context.histories.Add(history);
        };


            await _context.SaveChangesAsync();

            // return order id so caller (controller) can decide next step
            return new BaseResponse<int>(true, "Order created", order.Id);
        }

        private Address BuildAddress(CreateOrderDto dto, string userId)
        {
            return new Address
            {
                UserId = userId,
                City = dto.City,
                Country = dto.Country,
                Street = dto.Street
            };
        }

    }


}
