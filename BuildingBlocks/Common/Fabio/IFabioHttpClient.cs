using System.Threading.Tasks;

namespace U.Common.Fabio
{
    public interface IFabioHttpClient
    {
        Task<T> GetAsync<T>(string requestUri);
    }
}