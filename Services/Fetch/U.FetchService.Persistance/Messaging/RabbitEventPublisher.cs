using System.Threading.Tasks;
using RawRabbit;
using U.Common.EventPublisher;

namespace U.FetchService.Persistance.Messaging
{
    public class RabbitEventPublisher : IEventPublisher
    {
        private readonly IBusClient _busClient;

        public RabbitEventPublisher(IBusClient busClient)
        {
            _busClient = busClient;
        }

        public Task PublishMessage<T>(T msg)
        {
            return _busClient.BasicPublishAsync(msg, cfg => {
                cfg.OnExchange("ubiquitous").WithRoutingKey(typeof(T).Name.ToLower());
            });
        }
    }
}
