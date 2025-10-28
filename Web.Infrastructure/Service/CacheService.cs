using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Web.Infrastructure.Service
{
    public class CacheService(IDistributedCache distributedCache, ILogger<CacheService> logger) : ICacheService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;
        private readonly ILogger<CacheService> _logger = logger;

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            _logger.LogInformation("Get cache with key: {key}", key);

            var cachedValue = await _distributedCache.GetStringAsync(key);

            return cachedValue is null
                ? null
                : JsonSerializer.Deserialize<T>(cachedValue);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null) where T : class
        {
            _logger.LogInformation("Set cache with key: {key}", key);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration ?? TimeSpan.FromHours(6)
            };

            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
        }


        public async Task RemoveAsync(string key)
        {
            _logger.LogInformation("Remove cache with key: {key}", key);

            await _distributedCache.RemoveAsync(key);
        }
    }
}
