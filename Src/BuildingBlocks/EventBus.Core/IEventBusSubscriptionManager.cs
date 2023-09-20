namespace EventBus.Core
{
    public interface IEventBusSubscriptionManager
    {
        IReadOnlyDictionary<string, List<Type>> Handlers { get; }
        IReadOnlyList<Type> Events { get; }

        void RegisterEventHandler<TEvent, TEventHandler>()
            where TEvent : Event
            where TEventHandler : IEventHandler<TEvent>;
        void RegisterEvent<TEvent>()
            where TEvent : Event;
    }
}
