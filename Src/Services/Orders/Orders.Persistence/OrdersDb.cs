using Microsoft.EntityFrameworkCore;
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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDb).Assembly);
        }
    }
}