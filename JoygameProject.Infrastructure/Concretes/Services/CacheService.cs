using JoygameProject.Application.Abstractions.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace JoygameProject.Infrastructure.Concretes.Services
{
    public class CacheService(IDistributedCache cache) : ICacheService
    {

        public async Task<T?> GetAsync<T>(string key)
        {
            var data = await cache.GetStringAsync(key);
            return data == null ? default : JsonSerializer.Deserialize<T>(data);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expireTime = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expireTime ?? TimeSpan.FromMinutes(30)
            };

            var jsonData = JsonSerializer.Serialize(value);
            await cache.SetStringAsync(key, jsonData, options);
        }

        public async Task RemoveAsync(string key)
        {
            await cache.RemoveAsync(key);
        }
    }
}
