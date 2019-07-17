using System.Net.Http;
using System.Reflection;
using AutoMapper;
using MediatR;
using U.SmartStoreAdapter.Application.MappingProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RequestInjector.NetCore;
using SmartStore.Persistance.Context;
using Swashbuckle.AspNetCore.Swagger;
using U.Common.Installers;
using U.SmartStoreAdapter.Api.Products;
using U.SmartStoreAdapter.Application.Operations.Notifications;
using U.SmartStoreAdapter.Application.Operations.Products;
using U.SmartStoreAdapter.Middleware;
using IRequest = MediatR.IRequest;

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
        private IConfiguration Configuration { get; }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Mini profiler
            services.AddMiniProfiler();

            //Compatibility
            services.AddMvcCore()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            //Swagger 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "My API", Version = "v1"});
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

            #region MediatR register

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly,
                typeof(GetProductsListQueryHandler).GetTypeInfo().Assembly,
                typeof(FailedToStoreNotificationHandler).GetTypeInfo().Assembly);

            services.Scan(scan => scan
                .FromAssemblyOf<GetProductsListQuery>()
                .FromAssemblyOf<SuccessfulStoreNotificationHandler>()
                .AddClasses()
                .AsSelf()
                .WithTransientLifetime());

            var provider = services.BuildServiceProvider();

            services.AddMvc(config =>
                {
                    config.ModelMetadataDetailsProviders.Add(new RequestInjectorMetadataProvider());
                    config.ModelBinderProviders.Insert(0, new RequestInjectorModelBinderProvider());
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new RequestInjectorHandler<IRequest>(provider));
                });

            #endregion

            //DbContext            
            services
                .AddDatabaseOptionsAsSingleton(Configuration)
                .AddDatabaseContext<SmartStoreContext>();
            
            //Logging Behaviour Pipeline
            services.AddLoggingBehaviour();
            //Logging
            services.AddLogging();
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP transaction pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Database Integrator V1");
                c.RoutePrefix = string.Empty;
            });
            app.AddExceptionMiddleWare();
            app.UseHttpsRedirection();
            app.UseMiniProfiler();
            app.UseMvc();
        }
    }
}