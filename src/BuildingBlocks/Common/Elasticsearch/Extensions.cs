using System;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using U.Common.Mvc;
using U.Common.Telemetry.Elastic;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace DShop.Common.Logging
{
    public static class Extensions
    {
        public static IWebHostBuilder UseDistributedLogging(this IWebHostBuilder webHostBuilder, string applicationName)
            => webHostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                var elasticsearchOptions = context.Configuration.GetOptions<ElasticsearchOptions>("elastic");

                loggerConfiguration.Enrich.FromLogContext()
                    .MinimumLevel.Is(LogEventLevel.Information)
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("ApplicationName", applicationName);

                loggerConfiguration.WriteTo.Console();
                loggerConfiguration.WriteTo.ElasticsearchSink(context.Configuration);
            });

        public static void ElasticsearchSink(this LoggerSinkConfiguration loggerSinkConfiguration, IConfiguration configuration)
        {
            var elasticsearchOptions = configuration.GetOptions<ElasticsearchOptions>("elastic");

            if (elasticsearchOptions.Enabled)
            {
                loggerSinkConfiguration.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticsearchOptions.Url))
                    {
                        MinimumLogEventLevel = LogEventLevel.Information,
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                        IndexFormat = string.IsNullOrWhiteSpace(elasticsearchOptions.IndexFormat)
                            ? "logstash-{0:yyyy.MM.dd}"
                            : elasticsearchOptions.IndexFormat,
                        ModifyConnectionSettings = connectionConfiguration =>
                            elasticsearchOptions.BasicAuthEnabled
                                ? connectionConfiguration.BasicAuthentication(elasticsearchOptions.Username,
                                    elasticsearchOptions.Password)
                                : connectionConfiguration
                    });
            }
        }
    }
}