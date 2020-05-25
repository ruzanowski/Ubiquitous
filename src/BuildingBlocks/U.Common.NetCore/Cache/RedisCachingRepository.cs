using System;
using Microsoft.Extensions.Caching.Distributed;
using JsonSerializer = System.Text.Json.JsonSerializer;

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

        public T Get<T>(string id) where T : class
        {
            var cacheKey = GetCacheKey(id);
            var cached = _redisCache.GetString(cacheKey);

            return !(cached is null)
                ? JsonSerializer.Deserialize<T>(cached)
                : null;
        }

        public void Cache(string id, object toCache)
        {
            var cacheKey = GetCacheKey(id);
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(15))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

            var serialized = JsonSerializer.Serialize(toCache);

            _redisCache.SetString(cacheKey, serialized, options);
        }
    }
}