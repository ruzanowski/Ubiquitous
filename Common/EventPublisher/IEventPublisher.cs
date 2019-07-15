using System.Threading.Tasks;

namespace U.Common.EventPublisher
{
    public interface IEventPublisher
    {
        Task PublishMessage<T>(T msg);
    }
}
