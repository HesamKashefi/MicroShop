using EventBus.Core;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EventBus.RabbitMq
{
    public class RabbitMQBus : IEventBus
    {
        #region Fields
        private readonly Dictionary<string, List<Type>> _handlers = new();
        private readonly List<Type> _events = new();
        #endregion

        #region Dependencies
        private readonly ILogger<RabbitMQBus> _logger;
        #endregion

        #region Ctor
        public RabbitMQBus(ILogger<RabbitMQBus> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Implementation of IEventBus

        public void Publish<TEvent>(TEvent @event) where TEvent : Event
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var model = connection.CreateModel();

            var eventName = @event.GetType().Name;

            model.QueueDeclare(eventName, false, false, false, null);

            var body = JsonSerializer.SerializeToUtf8Bytes(@event);

            model.BasicPublish(string.Empty, eventName, null, body);
        }

        public void Subscribe<TEvent, TEventHandler>()
            where TEvent : Event
            where TEventHandler : IEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);
            if (!_events.Contains(eventType))
            {
                _events.Add(eventType);
            }

            if (!_handlers.ContainsKey(eventType.Name))
            {
                _handlers.Add(eventType.Name, new List<Type>());
            }

            if (_handlers[eventType.Name].Contains(typeof(TEventHandler)))
            {
                throw new InvalidOperationException($"Handler : {typeof(TEventHandler).Name} is already registered for Event : {eventType.Name}");
            }

            _handlers[eventType.Name].Add(typeof(TEventHandler));

            BasicConsume<TEvent, TEventHandler>();
        }

        #endregion

        #region Private Methods

        private void BasicConsume<TEvent, TEventHandler>()
            where TEvent : Event
            where TEventHandler : IEventHandler<TEvent>
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var model = connection.CreateModel();

            var eventName = typeof(TEvent).Name;

            model.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(model);
            consumer.Received += ConsumerOnReceived;

            model.BasicConsume(eventName, true, consumer);
        }

        private async Task ConsumerOnReceived(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var decoded = Encoding.UTF8.GetString(e.Body.ToArray());

            await ProcessEventAsync(eventName, decoded).ConfigureAwait(false);
        }

        private async Task ProcessEventAsync(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                var eventType = _events.Single(x => x.Name.Equals(eventName));
                var @event = JsonSerializer.Deserialize(message, eventType)!;

                var handlers = _handlers[eventName];

                foreach (var handlerType in handlers)
                {
                    try
                    {
                        var handler = Activator.CreateInstance(handlerType);
                        if (handler is null) continue;

                        await Task.Yield();

                        var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle")!.Invoke(handler, new object[] { @event })!;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Could not execute Handle on Handler {HandlerName} for EventType: {EventName} with Event: {@Event}", handlerType.Name, eventName, @event);
                    }
                }
            }
        }

        #endregion
    }
}