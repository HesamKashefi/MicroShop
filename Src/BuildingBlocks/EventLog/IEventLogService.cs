namespace EventLog
{
    public interface IEventLogService
    {
        Task<EventLogEntry[]> GetNonPublishedEventsAsync(CancellationToken cancellationToken);
        Task AddEventAsync(EventLogEntry eventLogEntry, CancellationToken cancellationToken);
        Task SetPublishedStatusForEventAsync(EventLogEntry eventLogEntry, CancellationToken cancellationToken);
        Task SetFailedStatusForEventAsync(EventLogEntry eventLogEntry, CancellationToken cancellationToken);
        Task SetInProgressStatusForEventAsync(EventLogEntry eventLogEntry, CancellationToken cancellationToken);
    }
}
