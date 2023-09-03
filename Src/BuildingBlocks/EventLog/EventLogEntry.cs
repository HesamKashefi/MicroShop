using EventBus.Core;
using Newtonsoft.Json;

namespace EventLog;

public class EventLogEntry
{
    /// <summary>
    /// Empty private constructor
    /// For EF Core
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private EventLogEntry() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public EventLogEntry(Event @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        EventId = @event.EventId;
        Content = JsonConvert.SerializeObject(@event);
        EventType = @event.GetType().Name;
        CreatedAt = DateTime.Now;
    }

    public Guid EventId { get; private set; }
    public string EventType { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public EventLogEntryStatus Status { get; private set; }


    public void SetInProgress()
    {
        if (Status != EventLogEntryStatus.Published)
        {
            Status = EventLogEntryStatus.InProgress;
        }
    }
    public void SetFailed()
    {
        if (Status != EventLogEntryStatus.Published && Status == EventLogEntryStatus.InProgress)
        {
            Status = EventLogEntryStatus.PublishFailed;
        }
    }
    public void SetPublished()
    {
        if (Status == EventLogEntryStatus.InProgress)
        {
            Status = EventLogEntryStatus.Published;
        }
    }
}
