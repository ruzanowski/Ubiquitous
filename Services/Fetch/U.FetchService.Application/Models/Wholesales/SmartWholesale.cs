using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using U.Common;
using U.FetchService.Application.Exceptions;
using U.SmartStoreAdapter.Api.Endpoints;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Models.Wholesales
{
    public class SmartWholesale : IWholesale
    {
        public PartySettings Settings { get; set; }
        private readonly ILogger<SmartWholesale> _logger;
        private readonly HttpClient _httpClient;
        public SmartWholesale(HttpClient httpClient, ILogger<SmartWholesale> logger, PartySettings partySettings)
        {
            _logger = logger;
            _httpClient = httpClient;
            Settings = partySettings;
        }

        public async Task<PaginatedItems<SmartProductViewModel>> FetchProducts()
        {
            try
            {
                var response = await _httpClient.GetAsync(EndpointsBoard.ProductEndpoint(Settings.Port));
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<PaginatedItems<SmartProductViewModel>>(content);
                return products;
            }
            catch (System.Net.Sockets.SocketException)
            {
                _logger.LogError($"Wholesale at port: {Settings.Port} is unavailable.");
                throw new FetchFailedException($"Wholesale at port: {Settings.Port} is unavailable.");
            }
            catch (HttpRequestException)
            {
                _logger.LogError($"Wholesale at port: {Settings.Port} is unavailable.");
                throw new FetchFailedException($"Wholesale at port: {Settings.Port} is unavailable.");
            }
        }
    }
}