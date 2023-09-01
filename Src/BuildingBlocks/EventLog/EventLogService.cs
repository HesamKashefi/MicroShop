using EventBus.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EventLog
{
    public class EventLogService : IEventLogService
    {
        #region Dependencies
        private readonly EventLogContext _eventLogContext;
        private readonly IEventBus _eventBus;
        private readonly IEventBusSubscriptionManager _eventBusSubscriptionManager;
        private readonly ILogger<EventLogService> _logger;
        #endregion

        #region Ctor
        public EventLogService(
           EventLogContext eventLogContext,
           IEventBus eventBus,
           IEventBusSubscriptionManager eventBusSubscriptionManager,
           ILogger<EventLogService> logger)
        {
            _eventLogContext = eventLogContext;
            _eventBus = eventBus;
            _eventBusSubscriptionManager = eventBusSubscriptionManager;
            _logger = logger;
        }
        #endregion

        #region Implementation of IEventLogService
        public async Task AddEventAsync(Event @event, IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            _eventLogContext.Database.UseTransaction(transaction.GetDbTransaction());
            _eventLogContext.Events.Add(new EventLogEntry(@event));
            await _eventLogContext.SaveChangesAsync(cancellationToken);
        }

        public async Task PublishPendingEventsAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            _eventLogContext.Database.UseTransaction(transaction.GetDbTransaction());

            var events = await GetNonPublishedEventsAsync(cancellationToken);
            var publishMethod = typeof(IEventBus).GetMethod(nameof(IEventBus.Publish))!;

            foreach (var @event in events)
            {
                _logger.LogTrace("Publishing event {@EventLogEntry}", @event);
                try
                {
                    @event.SetInProgress();
                    var eventType = _eventBusSubscriptionManager.Events.FirstOrDefault(x => x.Name == @event.EventType);
                    if (eventType is null)
                    {
                        _logger.LogWarning("Event Type not found for event {@EventLogEntry}", @event);
                        throw new Exception("EventType is not Registered : " + @event.EventType);
                    }
                    var eventObject = JsonSerializer.Deserialize(@event.Content, eventType)!;
                    var method = publishMethod.MakeGenericMethod(eventType);
                    method.Invoke(_eventBus, new object[] { eventObject });
                    @event.SetPublished();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to publish event {@EventLogEntry}", @event);
                    @event.SetFailed();
                }
            }

            await _eventLogContext.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region Private Methods
        private async Task<EventLogEntry[]> GetNonPublishedEventsAsync(CancellationToken cancellationToken)
        {
            return await _eventLogContext.Events
                .Where(x => x.Status == EventLogEntryStatus.NotPublished)
                .ToArrayAsync(cancellationToken);
        }
        #endregion
    }
}
