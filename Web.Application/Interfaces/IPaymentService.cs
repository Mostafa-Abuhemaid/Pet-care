using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.Response;

namespace Web.Application.Interfaces
{
    public  interface IPaymentService
    {
        Task<BaseResponse<string>> CreatePaymentIntentAsync(int orderid);
     }

    
}
