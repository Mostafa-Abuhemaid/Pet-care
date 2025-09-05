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
    public class OrderService(PricingService pricingService,AppDbContext context) : IOrderService
    {
        private readonly AppDbContext _context = context;
        private readonly PricingService _pricingService = pricingService;

        public async Task<BaseResponse<bool>> CreateOrderAsync(string userId, CreateOrderDto dto)
        {
            // 1. Build order
            var order = new Order
            {
                UserId = userId,
                Address = BuildAddress(dto, userId),
                PaymentMethod = dto.PaymentMethod,
                Status = OrderStatus.Pending
            };

            // 2. Handle payment method
            HandlePayment(dto);

            var cart = await _context.Carts
                .Include(x => x.PromoCode)
                .Include(x => x.Items).ThenInclude(i => i.Product)
                .SingleOrDefaultAsync(x => x.UserId == userId);

            if (cart == null || !cart.Items.Any())
                return new BaseResponse<bool>(false, "Cart is empty");

            var totals = _pricingService.CalculateCartTotals(cart, cart.PromoCode);

            order.Subtotal = totals.Subtotal;
            order.Tax = totals.Tax;
            order.Discount = totals.Discount;
            order.Total = totals.FinalTotal;

            await _context.orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return new BaseResponse<bool>(true, "Your order is on the way!");
        }

        private void HandlePayment(CreateOrderDto dto)
        {
            switch (dto.PaymentMethod)
            {
                case PaymentMethod.CashOnDelivery:
                    // nothing extra
                    break;

                case PaymentMethod.CreditCard:
                    var cardDto = dto as CreateCardOrderDto
                        ?? throw new ArgumentException("Invalid card DTO");
                    // Call Payment Gateway
                    break;

                case PaymentMethod.Wallet:
                    var walletDto = dto as CreateWalletOrderDto
                        ?? throw new ArgumentException("Invalid wallet DTO");
                    // Call Wallet API
                    break;
            }
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
