using EventBus.Core;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventLog
{
    public interface IEventLogService
    {
        Task AddEventAsync(Event @event, IDbContextTransaction transaction, CancellationToken cancellationToken);
        Task PublishPendingEventsAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);
    }
}
