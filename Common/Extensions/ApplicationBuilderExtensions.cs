using System;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using RawRabbit;
using U.Common.Notifications;

namespace U.Common.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddHandler<T>(this IApplicationBuilder app, IBusClient client) 
            where T : IMessage
        {
            if (!(app.ApplicationServices.GetService(typeof(IHandler<T>)) is IHandler<T> handler))
                throw new NullReferenceException();
        
            client.SubscribeAsync<T>(async (msg, context) =>
                {
                    await handler.HandleAsync(msg, CancellationToken.None);
                });
            return app;
        }
        public static IApplicationBuilder AddHandler<T>(this IApplicationBuilder app)
            where T : IMessage
        {
            if (!(app.ApplicationServices.GetService(typeof(IBusClient)) is IBusClient busClient))
                throw new NullReferenceException();
        
            return AddHandler<T>(app, busClient);
        }
    }
}