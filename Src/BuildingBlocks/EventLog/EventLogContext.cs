using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data.Common;

namespace EventLog
{
    public class EventLogContext : DbContext
    {
        public EventLogContext(DbConnection connection) : base(new DbContextOptionsBuilder<EventLogContext>().UseSqlServer(connection).Options)
        {
        }

        public DbSet<EventLogEntry> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventLogEntry>(ConfigureEventLogEntry);
        }

        private void ConfigureEventLogEntry(EntityTypeBuilder<EventLogEntry> builder)
        {
            builder.HasKey(x => x.EventId);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.EventType).IsRequired();
        }
    }
}
