using System;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using U.Common.Mvc;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace U.Common.Elasticsearch
{
    public static class Extensions
    {
        public static LoggerConfiguration ElasticsearchSink(this LoggerConfiguration loggerSinkConfiguration, IConfiguration configuration)
        {
            var elasticsearchOptions = configuration.GetOptions<ElasticsearchOptions>("elasticsearch");

            if (elasticsearchOptions.Enabled)
            {
                loggerSinkConfiguration.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticsearchOptions.Uri))
                    {
                        MinimumLogEventLevel = LogEventLevel.Debug,
                        AutoRegisterTemplate = true,
                        OverwriteTemplate = true,
                        DetectElasticsearchVersion = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                        RegisterTemplateFailure = RegisterTemplateRecovery.FailSink,
                        FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                           EmitEventFailureHandling.WriteToFailureSink |
                                           EmitEventFailureHandling.RaiseCallback,
                        ModifyConnectionSettings = connectionConfiguration =>
                            elasticsearchOptions.BasicAuthEnabled
                                ? connectionConfiguration.BasicAuthentication(elasticsearchOptions.Username,
                                    elasticsearchOptions.Password)
                                : connectionConfiguration
                    });
            }

            return loggerSinkConfiguration;
        }
    }
}