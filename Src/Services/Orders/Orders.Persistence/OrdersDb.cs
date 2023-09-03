using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Orders.Domain;

namespace Orders.Persistence
{
    public class OrdersDb : DbContext
    {
        public OrdersDb(DbContextOptions<OrdersDb> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; init; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Orders");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDb).Assembly);
        }
    }

    public class OrdersDbDesignTimeFactory : IDesignTimeDbContextFactory<OrdersDb>
    {
        public OrdersDb CreateDbContext(string[] args)
        {
            return new OrdersDb(new DbContextOptionsBuilder<OrdersDb>().UseSqlServer("server=(localdb)\\MSSQLLOCALDB;").Options);
        }
    }
}