using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Infrastructure.Service
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key)where T:class;
        Task SetAsync<T>(string key,T value)where T:class;
        Task RemoveAsync(string key);
    }
}
