using Mapster;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<BaseResponse<PromoCodeResponse>> CreatePromoCodeAsync(CreatePromoCodeDto dto)
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
            var response=promoCode.Adapt<PromoCodeResponse>();

            return new BaseResponse<PromoCodeResponse>(true,"Success", response);
        }

        public async Task<BaseResponse<IEnumerable<PromoCodeResponse>>> GetAllPromoCodeAsync()
        {
           var codes=await _context.promoCodes.AsNoTracking().ProjectToType<PromoCodeResponse>().ToListAsync();
            return new BaseResponse<IEnumerable<PromoCodeResponse>>(true,"Success",codes);
        }

        public async Task<BaseResponse<bool>> DeletePromoCodeAsync(int codeid)
        {
            if (await _context.promoCodes.FirstOrDefaultAsync(x => x.Id == codeid) is not { } code)
                return new BaseResponse<bool>(false, "Code is not found");

           _context.promoCodes.Remove(code);
            await _context.SaveChangesAsync();
            return new BaseResponse<bool>(true, "success");
        }

        public async Task<BaseResponse<bool>>ToggelStatusActive(int codeid)
        {
            var code=await _context.promoCodes.FirstOrDefaultAsync(x=>x.Id== codeid);
            code.IsActive=!code.IsActive;
            return new BaseResponse<bool>(true, "Sucess");
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
