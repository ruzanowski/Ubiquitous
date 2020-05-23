using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace U.Common.NetCore.Cache
{
    public class MemoryCachingRepository : ICachingRepository
    {
        private readonly IMemoryCache _redisCache;

        public MemoryCachingRepository(IMemoryCache redisCache)
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
            var cached = _redisCache.Get(cacheKey) as T;

            await Task.CompletedTask;

            return cached;
        }

        public async Task CacheAsync(string id, object toCache)
        {
            var cacheKey = GetCacheKey(id);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60));

            _redisCache.Set(cacheKey, toCache, cacheEntryOptions);
            await Task.CompletedTask;

        }
    }
}