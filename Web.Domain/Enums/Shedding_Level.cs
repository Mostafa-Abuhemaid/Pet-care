using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Web.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Shedding_Level
    {
        Low = 0,
        Moderate = 1,
        High = 2,
        Very_High = 3,
    }
}
