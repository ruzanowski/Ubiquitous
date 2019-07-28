using System.Net.Http;
using System.Reflection;
using AutoMapper;
using Consul;
using MediatR;
using U.SmartStoreAdapter.Application.MappingProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SmartStore.Persistance.Context;
using U.Common.Consul;
using U.Common.Database;
using U.Common.Fabio;
using U.Common.Pipeline;
using U.SmartStoreAdapter.Application.Operations.Products;
using U.SmartStoreAdapter.Middleware;

namespace U.SmartStoreAdapter
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Mini profiler
            services.AddMiniProfiler();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed(host => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            
            //Swagger 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo{Title = "My API", Version = "v1"});
                c.DescribeAllEnumsAsStrings();
            });

            //Services
            services.AddScoped<HttpClient>();

            #region Automapper profiles & init

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductMappingProfile());
                mc.AddProfile(new CategoryMappingProfile());
                mc.AddProfile(new ManufacturerMappingProfile());
                mc.AllowNullCollections = true;
            });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly,
                typeof(GetProductsListQueryHandler).GetTypeInfo().Assembly);

            //DbContext            
            services
                .AddDatabaseOptionsAsSingleton(Configuration)
                .AddDatabaseContext<SmartStoreContext>();
            
            //Logging Behaviour Pipeline
            services.AddLoggingBehaviour();
            //Logging
            services.AddLogging();
                
            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductMappingProfile());
                mc.AddProfile(new CategoryMappingProfile());
                mc.AddProfile(new ManufacturerMappingProfile());
            }).CreateMapper());

            services.AddCustomConsul();
            services.AddCustomFabio();
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP transaction pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime applicationLifetime, IConsulClient client)
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartStore Adapter V1");
                c.RoutePrefix = string.Empty;
            });
            app.AddExceptionMiddleWare();
            app.UseHttpsRedirection();
            app.UseMiniProfiler();
            app.UseMvc();
            
            var consulServiceId = app.UseCustomConsul();
            applicationLifetime.ApplicationStopped.Register(() => { client.Agent.ServiceDeregister(consulServiceId); });
        }
    }
}