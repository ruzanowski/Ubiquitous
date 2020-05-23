using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace U.Common.NetCore.Cache
{
    public class RedisCachingRepository : ICachingRepository
    {
        private readonly IDistributedCache _redisCache;

        public RedisCachingRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        private string GetCacheKey(string uniqueId)
        {
            return uniqueId;
        }

        public async Task<T> GetCachedOrDefaultAsync<T>(string id) where T : class
        {
            var cacheKey = GetCacheKey(id);
            var cached = await _redisCache.GetStringAsync(cacheKey);

            return !(cached is null)
                ? JsonConvert.DeserializeObject<T>(cached)
                : null;
        }

        public async Task CacheAsync(string id, object toCache)
        {
            var cacheKey = GetCacheKey(id);
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

            var serialized = JsonConvert.SerializeObject(toCache);

            await _redisCache.SetStringAsync(cacheKey, serialized, options);
        }
    }
}