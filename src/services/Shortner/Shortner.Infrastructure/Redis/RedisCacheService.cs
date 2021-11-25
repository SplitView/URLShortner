
using Microsoft.Extensions.Caching.Distributed;
using Shortner.Application.Interface;
using System.Text.Json;

namespace Shortner.Infrastructure.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            var result = await _cache.GetStringAsync(key);
            return result is null ? null : JsonSerializer.Deserialize<T>(result);
        }

        public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options) where T : class
        {
            var result = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, result, options);
        }

        public async Task ClearAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
