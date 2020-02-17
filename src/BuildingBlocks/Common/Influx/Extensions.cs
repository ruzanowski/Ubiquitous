using System;
using App.Metrics;
using App.Metrics.AspNetCore.Health;
using Microsoft.AspNetCore.Hosting;
using U.Common.Mvc;

namespace U.Common.Influx
{
    public static class Extensions
    {
        public static IWebHostBuilder UseInflux(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder
                .ConfigureMetricsWithDefaults((context, builder) =>
                {
                    var metricsOptions = context.Configuration.GetOptions<InfluxOptions>("influx");
                    if (!metricsOptions.Enabled)
                    {
                        return;
                    }

                    builder.Configuration.Configure(cfg =>
                        {
                            var tags = metricsOptions.Tags;
                            if (tags == null)
                            {
                                return;
                            }

                            tags.TryGetValue("app", out var app);
                            tags.TryGetValue("env", out var env);
                            tags.TryGetValue("server", out var server);
                            cfg.AddAppTag(string.IsNullOrWhiteSpace(app) ? null : app);
                            cfg.AddEnvTag(string.IsNullOrWhiteSpace(env) ? null : env);
                            cfg.AddServerTag(string.IsNullOrWhiteSpace(server) ? null : server);
                            foreach (var tag in tags)
                            {
                                if (!cfg.GlobalTags.ContainsKey(tag.Key))
                                {
                                    cfg.GlobalTags.Add(tag.Key, tag.Value);
                                }
                            }
                        }
                    );

                    builder.Report.ToInfluxDb(o =>
                    {
                        o.InfluxDb.Database = metricsOptions.Database;
                        o.InfluxDb.BaseUri = new Uri(metricsOptions.Uri);
                        o.InfluxDb.CreateDataBaseIfNotExists = true;
                        o.FlushInterval = TimeSpan.FromSeconds(metricsOptions.Interval);
                    });
                })
                .UseHealth()
                .UseHealthEndpoints()
                .UseMetricsWebTracking();
        }
    }
}