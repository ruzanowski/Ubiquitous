using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace U.Common.Pipeline
{
    public static class PipelineInstaller
    {
        public static IServiceCollection AddLoggingBehaviour(this IServiceCollection services)
            => services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    }
}