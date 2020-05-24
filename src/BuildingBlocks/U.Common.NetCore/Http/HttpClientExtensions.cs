using System.Net.Http;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace U.Common.NetCore.Http
{
    public static class HttpClientExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var dataAsString = await content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(dataAsString);
        }
    }
}