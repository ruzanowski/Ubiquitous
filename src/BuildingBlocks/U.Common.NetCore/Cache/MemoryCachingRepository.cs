using System;
using Microsoft.Extensions.Caching.Memory;

namespace U.Common.NetCore.Cache
{
    public class MemoryCachingRepository : ICachingRepository
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCachingRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        private string GetCacheKey(string uniqueId)
        {
            return uniqueId;
        }

        public T GetCachedOrDefault<T>(string id) where T : class
        {
            var cachedKey = GetCacheKey(id);
            _memoryCache.TryGetValue(cachedKey, out T cachedValue);

            return cachedValue;
        }

        public void Cache(string id, object toCache)
        {
            var cacheKey = GetCacheKey(id);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

            _memoryCache.Set(cacheKey, toCache, cacheEntryOptions);
        }
    }
}