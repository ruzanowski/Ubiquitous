using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace U.Common.NetCore.Mvc
{
    public static partial class Extensions
    {
        public static IApplicationBuilder UseServiceId(this IApplicationBuilder builder)
            => builder.Map("/id", c => c.Run(async ctx =>
            {
                using var scope = c.ApplicationServices.CreateScope();
                var id = scope.ServiceProvider.GetService<ISelfInfoService>().Id;
                await ctx.Response.WriteAsync(id);
            }));

        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();

            configuration.GetSection(section).Bind(model);

            return model;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddMvc().AddNewtonsoftJson();
            services.AddControllers(options => options.EnableEndpointRouting = false);
            services.AddSingleton<ISelfInfoService, SelfInfoService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }

        public static (IApplicationBuilder, string) UsePathBase<T>(this IApplicationBuilder app,
            IConfiguration configuration, ILogger<T> logger)
        {
            var pathBase = configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                logger.LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            return (app, pathBase);
        }
    }
}