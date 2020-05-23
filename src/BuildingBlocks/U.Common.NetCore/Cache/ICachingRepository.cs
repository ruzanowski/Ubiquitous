using System.Threading.Tasks;

namespace U.Common.NetCore.Cache
{
    public interface ICachingRepository
    {
        Task<T> GetCachedOrDefaultAsync<T>(string id) where T : class;
        Task CacheAsync(string id, object toCache);
    }
}