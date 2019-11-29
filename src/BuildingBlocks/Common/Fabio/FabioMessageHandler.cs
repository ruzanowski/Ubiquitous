using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Polly;
using U.Common.Jwt;

namespace U.Common.Fabio
{
    public class FabioMessageHandler : DelegatingHandler
    {
        private readonly IOptions<FabioOptions> _options;
        private readonly string _servicePath;
        private readonly HttpContext _httpContext;

        public FabioMessageHandler(IOptions<FabioOptions> options, IHttpContextAccessor httpContextAccessor, string serviceName = null)
        {
            if (string.IsNullOrWhiteSpace(options.Value.Url))
            {
                throw new InvalidOperationException("Fabio URL was not provided.");
            }

            _options = options;
            _httpContext = httpContextAccessor.HttpContext;
            _servicePath = string.IsNullOrWhiteSpace(serviceName) ? string.Empty : $"{serviceName}/";
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.RequestUri = GetRequestUri(request);

            foreach (var header in _httpContext.Request.Headers)
            {
                request.Headers.Add(header.Key, header.Value.ToArray());
            }

            return await Policy.Handle<Exception>()
                .WaitAndRetryAsync(RequestRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await base.SendAsync(request, cancellationToken));
        }

        private Uri GetRequestUri(HttpRequestMessage request)
            =>  new Uri($"{_options.Value.Url}/{_servicePath}{request.RequestUri.Host}{request.RequestUri.PathAndQuery}");

        private int RequestRetries => _options.Value.RequestRetries <= 0 ? 3 : _options.Value.RequestRetries;
    }
}