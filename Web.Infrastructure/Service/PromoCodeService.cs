using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.CartDTO;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Domain.Entites;
using Web.Infrastructure.Persistence.Data;

namespace Web.Infrastructure.Service
{
    public class PromoCodeService(AppDbContext context) : IPromoCodeService
    {
        private readonly AppDbContext _context = context;
        private static readonly Random _random = new Random();

        public async Task<BaseResponse<decimal>> ApplyPromoCode(CartResponse response, string code)
        {
            var promo = await _context.promoCodes
                .FirstOrDefaultAsync(p => p.Code == code && p.IsActive);

            if (promo == null ||
                DateTime.Now < promo.StartDate ||
                DateTime.Now > promo.EndDate ||
                (promo.MaxUsageCount > 0 && promo.UsedCount >= promo.MaxUsageCount))
            {
                return new BaseResponse<decimal>(false, "Invalid or expired promo code");
            }

            decimal totalBeforePromo = (decimal)response.TotalPayment;

            decimal discount = promo.DiscountValue;

            if (discount > totalBeforePromo)
                discount = totalBeforePromo; 

            decimal totalAfterDiscount = totalBeforePromo - discount;

            promo.UsedCount++;
            await _context.SaveChangesAsync();

            return new BaseResponse<decimal>(true, "Promo code applied successfully", totalAfterDiscount);
        }

        public async Task<PromoCode> CreatePromoCodeAsync(CreatePromoCodeDto dto)
        {
            string code = await GenerateUniqueCodeAsync();

            var promoCode = new PromoCode
            {
                Code = code,
                DiscountValue = dto.DiscountValue,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                MaxUsageCount = dto.MaxUsageCount,
                UsedCount = 0,
                IsActive = true
            };

            _context.promoCodes.Add(promoCode);
            await _context.SaveChangesAsync();

            return promoCode;
        }

        private async Task<string> GenerateUniqueCodeAsync()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string code;
            bool exists;

            do
            {
                code = new string(Enumerable.Range(0, 6)
                            .Select(_ => chars[_random.Next(chars.Length)]).ToArray());

                exists = await _context.promoCodes.AnyAsync(p => p.Code == code);
            }
            while (exists);

            return code;
        }

    }
}
