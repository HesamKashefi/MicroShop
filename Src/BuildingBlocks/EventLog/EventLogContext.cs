using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventLog
{
    public class EventLogContext : DbContext
    {
        public EventLogContext(DbContextOptions<EventLogContext> options) : base(options)
        {
        }

        public DbSet<EventLogEntry> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("EventLogs");
            modelBuilder.Entity<EventLogEntry>(ConfigureEventLogEntry);
        }

        private void ConfigureEventLogEntry(EntityTypeBuilder<EventLogEntry> builder)
        {
            builder.HasKey(x => x.EventId);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.EventType).IsRequired();
        }
    }

    public class EventLogContextDesignTimeFactory : IDesignTimeDbContextFactory<EventLogContext>
    {
        public EventLogContext CreateDbContext(string[] args)
        {
            return new EventLogContext(new DbContextOptionsBuilder<EventLogContext>().UseSqlServer("server=(localdb)\\MSSQLLOCALDB;").Options);
        }
    }
}
