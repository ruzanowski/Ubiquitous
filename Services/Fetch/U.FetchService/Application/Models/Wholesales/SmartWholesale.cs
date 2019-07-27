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
                var url = EndpointsBoard.GetProductListUrl(Settings.Ip, Settings.Port);
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }
                
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<PaginatedItems<SmartProductViewModel>>(content);
                return products;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                _logger.LogError($"Service at '{Settings.Ip}:{Settings.Port}' is unavailable.");
                throw new FetchFailedException($"Wholesale at '{Settings.Ip}:{Settings.Port}' is unavailable.", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Service at '{Settings.Ip}:{Settings.Port}' is bad.");
                throw new FetchFailedException($"Wholesale at '{Settings.Ip}:{Settings.Port}' is bad.", ex);
            }
        }
    }
}