using MediatR;
using Microsoft.Extensions.DependencyInjection;
using U.Common.Infrastructure;

namespace U.Common.Installers
{
    public static class PipelineInstaller
    {
        public static void AddLoggingBehaviour(this IServiceCollection services)
            => services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    }
}