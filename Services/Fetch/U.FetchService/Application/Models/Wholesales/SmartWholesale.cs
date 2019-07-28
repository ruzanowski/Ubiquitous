using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using U.Common.Pagination;
using U.FetchService.Application.Exceptions;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Models.Wholesales
{
    public class SmartWholesale : IWholesale
    {
        public PartySettings Settings { get; set; }
        private readonly ILogger<SmartWholesale> _logger;
        private readonly HttpClient _httpClient;

        private static string EndpointBase { get; } = "/api/smartstore/";
        private static string Products { get; } = "products";

        private static string GetProductListUrl(string baseUrl, int port) => $"{GetCorrectUrl(baseUrl)}:{port}{EndpointBase}{Products}/get-list";
        private static string GetCorrectUrl(string baseUrl) => $"{(baseUrl.StartsWith("http://") ? baseUrl : $"http://{baseUrl}")}";

        public SmartWholesale(HttpClient httpClient, ILogger<SmartWholesale> logger, PartySettings partySettings)
        {
            _logger = logger;
            _httpClient = httpClient;
            Settings = partySettings;
        }

        public async Task<PaginatedItems<SmartProductViewModel>> FetchProducts()
        {
            var url = GetProductListUrl(Settings.Ip, Settings.Port);
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Fetching from '{url}' failed." +
                                 $"{Environment.NewLine}" +
                                 $"Http response: {response.StatusCode}" +
                                 $"{Environment.NewLine}" +
                                 $"Http response message: {response.Content.ReadAsStringAsync()}"
                );
                throw new FetchFailedException();
            }

            var content = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<PaginatedItems<SmartProductViewModel>>(content);
            return products;
        }
    }
}