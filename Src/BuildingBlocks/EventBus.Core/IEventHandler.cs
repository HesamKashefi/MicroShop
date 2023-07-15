namespace EventBus.Core
{
    public interface IEventHandler<in TEvent> where TEvent : Event
    {
        Task Handle(TEvent @event);
    }
}
