namespace U.Common.NetCore.Cache
{
    public interface ICachingRepository
    {
        T Get<T>(string id);
        void Cache(string id, object toCache);
        void Delete(string id);
    }
}