using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using U.Common.Behaviour;

namespace U.Common.Mvc
{
    public static class MvcExtensions
    {
        public static IApplicationBuilder UseServiceId(this IApplicationBuilder builder)
            => builder.Map("/id", c => c.Run(async ctx =>
            {
                using (var scope = c.ApplicationServices.CreateScope())
                {
                    var id = scope.ServiceProvider.GetService<IServiceId>().Id;
                    await ctx.Response.WriteAsync(id);
                }
            }));

        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            
            configuration.GetSection(section).Bind(model);
            
            return model;
        }
        
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            // AddAsync framework services.
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices(); //Injecting Controllers themselves thru DI
            //For further info see: http://docs.autofac.org/en/latest/integration/aspnetcore.html#controllers-as-services

            string[] allowedOrigins =
            {
                "http://localhost:4200",
                "http://docker.for.win.localhost:4200"
            };
 
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed(host => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins(allowedOrigins));
            });
            
            services.AddSingleton<IServiceId, ServiceId>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
        
        public static IServiceCollection AddLoggingBehaviour(this IServiceCollection services)
            => services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

        public static (IApplicationBuilder,string) UseCustomPathBase<T>(this IApplicationBuilder app, IConfiguration configuration, ILogger<T> logger)
        {
            var pathBase = configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                logger.LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            return (app, pathBase);
        }
        
        public static IApplicationBuilder UseAllForwardedHeaders(this IApplicationBuilder builder)
            => builder.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

    }
}