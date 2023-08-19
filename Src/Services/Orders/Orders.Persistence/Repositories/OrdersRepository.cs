using Microsoft.EntityFrameworkCore;
using Orders.Domain;
using Orders.Domain.Contracts;

namespace Orders.Persistence.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrdersDb _db;

        public OrdersRepository(OrdersDb db)
        {
            _db = db;
        }

        public async Task<Order[]> GetBuyerOrdersAsync(int buyerId)
        {
            return await _db.Orders
                .Where(order => order.BuyerId == buyerId)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<Order?> GetOrderAsync(int orderId)
        {
            return await _db.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(order => order.BuyerId == orderId);
        }

        public async Task SaveOrderAsync(Order order)
        {
            if (order.Id == 0)
            {
                _db.Orders.Add(order);
            }
            else
            {
                _db.Entry(order).State = EntityState.Modified;
            }
            await _db.SaveChangesAsync();
        }
    }
}
