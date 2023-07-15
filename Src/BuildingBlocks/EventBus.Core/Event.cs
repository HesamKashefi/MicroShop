namespace EventBus.Core
{
    public abstract class Event
    {
        public DateTime CaptureTime { get; init; } = DateTime.Now;
    }
}