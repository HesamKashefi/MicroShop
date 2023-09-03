using EventLog;
using Microsoft.EntityFrameworkCore;
using Orders.Persistence;

namespace Orders.Api.Models
{
    public static class OrdersExtensions
    {
        public static async Task DbInitAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<OrdersDb>();
            await db.Database.MigrateAsync();


            using var eventLogContext = scope.ServiceProvider.GetRequiredService<EventLogContext>();
            await eventLogContext.Database.MigrateAsync();
        }
    }
}
