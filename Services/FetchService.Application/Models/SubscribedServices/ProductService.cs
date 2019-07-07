using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using U.FetchService.Application.Extensions;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Models.SubscribedServices
{
    public class ProductService : IService
    {
        public PartySettings Settings { get; set; }
        private readonly ILogger<ProductService> _logger;
        private readonly HttpClient _httpClient;
        public ProductService(HttpClient httpClient, ILogger<ProductService> logger, PartySettings partySettings)
        {
            _logger = logger;
            _httpClient = httpClient;
            Settings = partySettings;
        }

        
        //todo: change to ProductService dto model.
        public async Task<bool> ForwardData(IEnumerable<SmartProductViewModel> products)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/productservice/", products);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                _logger.LogError($"Service at port: {Settings.Port} is unavailable.");
                return false;
            }
        }
    }
}