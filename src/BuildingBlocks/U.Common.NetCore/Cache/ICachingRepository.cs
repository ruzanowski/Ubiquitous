namespace U.Common.NetCore.Cache
{
    public interface ICachingRepository
    {
        T Get<T>(string id) where T : class;
        void Cache(string id, object toCache);
    }
}