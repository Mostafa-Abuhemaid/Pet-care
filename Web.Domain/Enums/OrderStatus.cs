using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Enums
{

    public enum OrderStatus
    {
        Pending,     // just created, awaiting processing
        Paid,        // if card/wallet success
        Shipped,
        Delivered,
        Cancelled
    }

}
