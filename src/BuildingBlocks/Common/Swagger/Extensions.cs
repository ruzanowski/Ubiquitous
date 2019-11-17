using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using U.Common.Mvc;

namespace U.Common.Swagger
{
    public static class Extensions
    {
        private static string SwaggerSectionName = "swagger";

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var swaggerOptions = configuration.GetOptions<SwaggerOptions>(SwaggerSectionName);

            services.AddSingleton(swaggerOptions);

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = swaggerOptions.Title,
                    Version = swaggerOptions.Version,
                    Description = swaggerOptions.Description,
                    Contact = new OpenApiContact
                    {
                        Name = "Sebastian Rużanowski",
                        Email = swaggerOptions.Contact
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT"
                    }
                });

                // Bearer token authentication
                OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                };
                options.AddSecurityDefinition("jwt_auth", securityDefinition);

                // Make sure swagger UI requires a Bearer token specified
                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Id = "jwt_auth",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                {
                    {securityScheme, new string[] { }},
                };
                options.AddSecurityRequirement(securityRequirements);
            });

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, string pathBase)
        {
            var options = app.ApplicationServices.GetService<SwaggerOptions>();

            return app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(
                        $"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/v1/swagger.json",
                        options.Title);
                });
        }
    }
}