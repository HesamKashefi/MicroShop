using EventBus.Core;
using Microsoft.EntityFrameworkCore;

namespace EventLog
{
    public class EventLogService : IEventLogService
    {
        private readonly EventLogContext _eventLogContext;

        public EventLogService(EventLogContext eventLogContext)
        {
            _eventLogContext = eventLogContext;
        }

        public async Task AddEventAsync(Event @event, CancellationToken cancellationToken)
        {
            _eventLogContext.Events.Add(new EventLogEntry(@event));
            await _eventLogContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<EventLogEntry[]> GetNonPublishedEventsAsync(CancellationToken cancellationToken)
        {
            return await _eventLogContext.Events
                .Where(x => x.Status == EventLogEntryStatus.NotPublished)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }

        public async Task SetFailedStatusForEventAsync(EventLogEntry eventLogEntry, CancellationToken cancellationToken)
        {
            eventLogEntry.SetFailed();
            if (eventLogEntry.Status == EventLogEntryStatus.PublishFailed)
            {
                throw new Exception("EventLogEntry is in a wrong state!");
            }
            _eventLogContext.Entry(eventLogEntry).State = EntityState.Modified;
            await _eventLogContext.SaveChangesAsync(cancellationToken);
        }

        public async Task SetInProgressStatusForEventAsync(EventLogEntry eventLogEntry, CancellationToken cancellationToken)
        {
            eventLogEntry.SetInProgress();
            if (eventLogEntry.Status == EventLogEntryStatus.InProgress)
            {
                throw new Exception("EventLogEntry is in a wrong state!");
            }
            _eventLogContext.Entry(eventLogEntry).State = EntityState.Modified;
            await _eventLogContext.SaveChangesAsync(cancellationToken);
        }

        public async Task SetPublishedStatusForEventAsync(EventLogEntry eventLogEntry, CancellationToken cancellationToken)
        {
            eventLogEntry.SetPublished();
            if (eventLogEntry.Status == EventLogEntryStatus.Published)
            {
                throw new Exception("EventLogEntry is in a wrong state!");
            }
            _eventLogContext.Entry(eventLogEntry).State = EntityState.Modified;
            await _eventLogContext.SaveChangesAsync(cancellationToken);
        }
    }
}
