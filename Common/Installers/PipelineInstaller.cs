using System.Collections;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using U.Common.Behaviours;

namespace U.Common.Installers
{
    public static class PipelineInstaller
    {
        public static IServiceCollection AddLoggingBehaviour(this IServiceCollection services)
            => services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    }
}