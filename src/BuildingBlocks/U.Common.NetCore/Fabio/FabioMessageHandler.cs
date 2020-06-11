using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using U.Common.NetCore.Auth.Claims;

namespace U.Common.NetCore.Fabio
{
    public class FabioMessageHandler : DelegatingHandler
    {
        private readonly IOptions<FabioOptions> _options;
        private readonly string _servicePath;
        private readonly IServiceProvider _provider;

        public FabioMessageHandler(IOptions<FabioOptions> options, IServiceProvider provider, string serviceName = null)
        {
            if (string.IsNullOrWhiteSpace(options.Value.Url))
            {
                throw new InvalidOperationException("Fabio URL was not provided.");
            }

            _options = options;
            _provider = provider;
            _servicePath = string.IsNullOrWhiteSpace(serviceName) ? string.Empty : $"{serviceName}/";
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.RequestUri = GetRequestUri(request);


            var httpContext = _provider.CreateScope().ServiceProvider.GetService<HttpContext>();

            var token = httpContext.GetAccessToken();

            if (httpContext is null)
            {
                var signalRContext = _provider.CreateScope().ServiceProvider.GetService<HubCallerContext>();

                token = signalRContext.GetAccessToken();
            }

            request.Headers.Add("Authorization", "Bearer " + token);

            return await Policy.Handle<Exception>()
                .WaitAndRetryAsync(RequestRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await base.SendAsync(request, cancellationToken));
        }

        private Uri GetRequestUri(HttpRequestMessage request)
            =>  new Uri($"{_options.Value.Url}/{_servicePath}{request.RequestUri.Host}{request.RequestUri.PathAndQuery}");

        private int RequestRetries => _options.Value.RequestRetries <= 0 ? 3 : _options.Value.RequestRetries;
    }
}