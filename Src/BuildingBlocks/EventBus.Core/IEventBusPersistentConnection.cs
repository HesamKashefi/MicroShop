namespace EventBus.Core
{
    public interface IEventBusPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        event EventBusConnectionEventHandler? OnConnected;
    }
    public record EventBusConnectionEventArgs();
    public delegate void EventBusConnectionEventHandler(object sender, EventBusConnectionEventArgs e);
}
