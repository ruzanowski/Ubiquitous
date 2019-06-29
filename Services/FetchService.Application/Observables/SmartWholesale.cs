using System.Net.Http;
using Microsoft.Extensions.Logging;
using U.FetchService.Application.Models;

namespace U.FetchService.Application.Observables
{
    public class SmartWholesale : SmartBase, IWholesale
    {
        public SmartWholesale(HttpClient httpClient, int port, ILogger<SmartWholesale> logger) : base(httpClient, port, logger)
        {
        }
    }
}