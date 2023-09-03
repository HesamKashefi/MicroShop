namespace EventBus.Core
{
    public interface IEventBusSubscriptionManager
    {
        IReadOnlyDictionary<string, List<Type>> Handlers { get; }
        IReadOnlyList<Type> Events { get; }

        void Subscribe<TEvent, TEventHandler>()
            where TEvent : Event
            where TEventHandler : IEventHandler<TEvent>;
        void RegisterEventType<TEvent>()
            where TEvent : Event;
    }
}
