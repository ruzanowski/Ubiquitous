using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace U.ProductService.Persistance.Repositories
{
    public abstract class CachingRepository
    {
        private readonly IDistributedCache _cache;

        protected CachingRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        private string GetCacheKey<T>(string uniqueId)
        {
            return uniqueId;
        }

        protected async Task<T> GetCachedOrDefaultAsync<T>(string id) where T : class
        {
            var cacheKey = GetCacheKey<T>(id);
            var cached = await _cache.GetStringAsync(cacheKey);

            return !(cached is null)
                ? JsonConvert.DeserializeObject<T>(cached)
                : null;
        }

        protected async Task CacheAsync<T>(string id, T toCache) where T : class
        {
            var cacheKey = GetCacheKey<T>(id);
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(toCache), options);
        }
    }
}