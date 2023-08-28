namespace EventBus.Core
{
    /// <summary>
    /// Represents an Event in a service
    /// </summary>
    public abstract class Event
    {
        /// <summary>
        /// Id of the event
        /// </summary>
        public Guid EventId { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// The time of event
        /// </summary>
        public DateTime CaptureTime { get; init; } = DateTime.Now;
    }
}