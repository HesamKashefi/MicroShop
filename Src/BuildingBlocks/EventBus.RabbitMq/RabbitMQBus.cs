using EventBus.Core;
using Microsoft.Extensions.DependencyInjection;
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
        private IReadOnlyDictionary<string, List<Type>> _handlers => _eventBusSubscriptionManager.Handlers;
        private IReadOnlyList<Type> _events => _eventBusSubscriptionManager.Events;
        #endregion

        #region Dependencies
        private readonly IRabbitMQPersistentConnection _rabbitMQPersistentConnection;
        private readonly IEventBusSubscriptionManager _eventBusSubscriptionManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RabbitMQBus> _logger;
        #endregion

        #region Ctor
        public RabbitMQBus(
            IRabbitMQPersistentConnection rabbitMQPersistentConnection,
            IEventBusSubscriptionManager eventBusSubscriptionManager,
            IServiceProvider serviceProvider,
            ILogger<RabbitMQBus> logger)
        {
            _rabbitMQPersistentConnection = rabbitMQPersistentConnection ?? throw new ArgumentNullException(nameof(rabbitMQPersistentConnection));
            _eventBusSubscriptionManager = eventBusSubscriptionManager ?? throw new ArgumentNullException(nameof(eventBusSubscriptionManager));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            rabbitMQPersistentConnection.OnConnected += delegate
            {
                foreach (var @event in _events)
                    BasicConsume(@event);
            };
        }
        #endregion

        #region Implementation of IEventBus

        public void Publish<TEvent>(TEvent @event) where TEvent : Event
        {
            if (!_rabbitMQPersistentConnection.IsConnected && !_rabbitMQPersistentConnection.TryConnect())
            {
                throw new Exception("RabbitMQ Connection Failed");
            }
            var model = _rabbitMQPersistentConnection.CreateModel();

            var eventName = @event.GetType().Name;

            model.QueueDeclare(eventName, false, false, false, null);

            var body = JsonSerializer.SerializeToUtf8Bytes(@event);

            model.BasicPublish(string.Empty, eventName, null, body);
        }

        public void Subscribe<TEvent, TEventHandler>()
            where TEvent : Event
            where TEventHandler : IEventHandler<TEvent>
        {
            _eventBusSubscriptionManager.Subscribe<TEvent, TEventHandler>();
        }

        #endregion

        #region Private Methods

        private void BasicConsume(Type eventType)
        {
            if (!_rabbitMQPersistentConnection.IsConnected && !_rabbitMQPersistentConnection.TryConnect())
            {
                throw new Exception("RabbitMQ Connection Failed");
            }
            var model = _rabbitMQPersistentConnection.CreateModel();

            var eventName = eventType.Name;

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
            _logger.LogTrace("Processing Event {Event} With Message : {Message}", eventName, message);
            if (_handlers.ContainsKey(eventName))
            {
                var eventType = _events.Single(x => x.Name.Equals(eventName));
                var @event = JsonSerializer.Deserialize(message, eventType)!;

                var handlers = _handlers[eventName];

                using var scope = _serviceProvider.CreateScope();

                foreach (var handlerType in handlers)
                {
                    try
                    {
                        var handler = scope.ServiceProvider.GetService(handlerType);
                        if (handler is null)
                        {
                            throw new InvalidOperationException($"Handler: {handlerType.Name} for Event Type: {eventType.Name} Was not registered with the DI Container");
                        }

                        _logger.LogTrace("Processing Event {Event} Using Handler: {EventHandler} With Message : {Message}", eventName, handlerType.Name, message);
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