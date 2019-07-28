using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed(host => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            return services;
        }

    }
}