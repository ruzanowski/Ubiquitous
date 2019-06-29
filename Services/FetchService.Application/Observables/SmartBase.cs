using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using U.FetchService.Application.Models;
using U.SmartStoreAdapter.Api.Endpoints;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Observables
{
    public abstract class SmartBase
    {
        private readonly HttpClient _httpClient;
        private int Port { get; }
        private readonly ILogger<SmartBase> _logger;

        protected SmartBase(HttpClient httpClient, int port, ILogger<SmartBase> logger)
        {    
            _httpClient = httpClient;
            Port = port;
            _logger = logger;
        }
        
        public async Task<PaginatedItems<SmartProductViewModel>> FetchProducts()
        {
            try
            {
                var response = await _httpClient.GetAsync(EndpointsBoard.ProductEndpoint(Port));
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<PaginatedItems<SmartProductViewModel>>(content);
                return products;
            }
            catch (System.Net.Sockets.SocketException)
            {
                _logger.LogError($"Wholesale at port: {Port} is unavailable.");
                return null;
            }
            catch (HttpRequestException)
            {
                _logger.LogError($"Wholesale at port: {Port} is unavailable.");
                return null;
            }
        }
    }
}