using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Web.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BookingStatus
    {
        Pending = 0,
        Confirmed = 1,
        Failed = 2,
        Cancelled = 3
    }
}
