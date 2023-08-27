using EventBus.Core;
using RabbitMQ.Client;

namespace EventBus.RabbitMq
{
    /// <summary>
    /// Holds a connection to RabbitMQ
    /// </summary>
    public interface IRabbitMQPersistentConnection : IEventBusPersistentConnection
    {
        /// <summary>
        /// Creates a RabbitMQ Model
        /// </summary>
        /// <returns>RabbitMQ Model</returns>
        IModel CreateModel();
    }
}
