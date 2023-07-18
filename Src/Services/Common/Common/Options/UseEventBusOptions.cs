using EventBus.Core;
using System.Collections.Immutable;

namespace Common.Options
{
    public class UseEventBusOptions
    {
        private readonly Dictionary<Type, List<Type>> _handlers = new();
        public ImmutableDictionary<Type, IReadOnlyCollection<Type>> Handlers => _handlers.ToImmutableDictionary(x => x.Key, x => x.Value.AsReadOnly() as IReadOnlyCollection<Type>);

        public UseEventBusOptions Subscribe<TEvent, TEventHandler>() where TEvent : Event where TEventHandler : IEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);
            var eventHandlerType = typeof(TEventHandler);

            if (!_handlers.ContainsKey(eventType))
            {
                _handlers.Add(eventType, new());
            }

            if (!_handlers[eventType].Contains(eventHandlerType))
            {
                _handlers[eventType].Add(eventHandlerType);
            }

            return this;
        }
    }
}