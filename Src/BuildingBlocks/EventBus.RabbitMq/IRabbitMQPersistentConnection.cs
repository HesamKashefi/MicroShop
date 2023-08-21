using EventBus.Core;
using RabbitMQ.Client;

namespace EventBus.RabbitMq
{
    public interface IRabbitMQPersistentConnection : IEventBusPersistentConnection
    {
        IModel CreateModel();
    }
}
