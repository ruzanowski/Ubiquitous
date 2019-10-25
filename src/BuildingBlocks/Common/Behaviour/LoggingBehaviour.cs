using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using U.Common.Extensions;

namespace U.Common.Behaviour
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogDebug($"----- Handling command {request.GetGenericTypeName()}");
            var response = await next();
            _logger.LogDebug($"----- Command {request.GetGenericTypeName()} handled");

            return response;
        }
    }
}
