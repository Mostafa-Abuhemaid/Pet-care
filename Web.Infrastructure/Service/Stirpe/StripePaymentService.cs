using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.V2;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Domain.Entites;
using Web.Domain.Enums;
using Web.Infrastructure.Persistence.Data;
using Web.Infrastructure.Service.Stripe;

public class StripePaymentService(IOptions<StripeSettings> options,AppDbContext context) : IPaymentService
{
    private readonly StripeSettings _options = options.Value;
    private readonly AppDbContext _context = context;

    public async Task<BaseResponse<string>> CreatePaymentIntentAsync(int orderid)
    {
        if (_context.orders.SingleOrDefault(x => x.Id == orderid) is not { } order)
            return new BaseResponse<string>(false, "Cart is Null!");

        var client = new StripeClient(_options.SecretKey); // ✅ هنا بتمرر المفتاح
        var service = new PaymentIntentService(client);

        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(order.Total * 100),
            Currency = _options.Currency,
            PaymentMethodTypes = new List<string> { "card" }
        };

        var intent = await service.CreateAsync(options);

        var payment = new Payment
        {
            OrderId = orderid,
            Amount = order.Total,
            Currency = _options.Currency,
            PaymentMethod = "Card",
            StripePaymentIntentId = intent.Id,
            Status = intent.Status,
            CreatedAt = DateTime.UtcNow
        };

        await _context.AddAsync(payment);
        await _context.SaveChangesAsync();



        return new BaseResponse<string>(true, "Successed", intent.ClientSecret);
     // ده اللي هنبعته للـ Frontend
}

}
