using Microsoft.Extensions.Logging;

namespace EventBus.Core
{
    public class DefaultEventBusSubscriptionManager : IEventBusSubscriptionManager
    {
        #region Fields

        private readonly Dictionary<string, List<Type>> _handlers = new();
        private readonly List<Type> _events = new();

        #endregion

        #region Dependencies

        private readonly ILogger<DefaultEventBusSubscriptionManager> _logger;

        #endregion

        #region Ctor

        public DefaultEventBusSubscriptionManager(ILogger<DefaultEventBusSubscriptionManager> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Implementation of IEventBusSubscriptionManager

        public IReadOnlyDictionary<string, List<Type>> Handlers => _handlers.AsReadOnly();

        public IReadOnlyList<Type> Events => _events.AsReadOnly();

        public void RegisterEventType<TEvent>() where TEvent : Event
        {
            var eventType = typeof(TEvent);
            if (!_events.Contains(eventType))
            {
                _events.Add(eventType);
            }
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
            _logger.LogTrace("Registered Handler : {Handler} For Event: {Event}", typeof(TEventHandler).Name, eventType.Name);
        }

        #endregion
    }
}
