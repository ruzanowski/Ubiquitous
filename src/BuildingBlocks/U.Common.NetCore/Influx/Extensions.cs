using App.Metrics;
using Microsoft.AspNetCore.Hosting;

namespace U.Common.NetCore.Influx
{
    public static class Extensions
    {
        public static IMetricsRoot Metrics { get; set; }

        public static IWebHostBuilder UseInflux(this IWebHostBuilder webHostBuilder)
        {
            // Metrics = AppMetrics.CreateDefaultBuilder()
            //     .OutputMetrics.AsJson()
            //     .OutputMetrics.AsPlainText()
            //     .Build();

            return webHostBuilder;

            //todo: INFLUX
            //WEBHOSTBUILDER VS HOSTBUILDER

            // .ConfigureHealth((context, builder) =>
            // {
            //     var metricsOptions = context.Configuration.GetOptions<InfluxOptions>("influx");
            //     if (!metricsOptions.Enabled)
            //     {
            //         return;
            //     }
            //     builder.Report.ToInfluxDb(o =>
            //     {
            //         o.InfluxDb.Database = metricsOptions.Database;
            //         o.InfluxDb.BaseUri = new Uri(metricsOptions.Uri);
            //         o.InfluxDb.CreateDataBaseIfNotExists = true;
            //         o.FlushInterval = TimeSpan.FromSeconds(metricsOptions.Interval);
            //     });
            // })
            // .UseMetricsWebTracking();
        }
    }
}