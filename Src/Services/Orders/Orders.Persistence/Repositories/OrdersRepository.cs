using Orders.Domain;
using Orders.Domain.Contracts;

namespace Orders.Persistence.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        public Task<Order[]> GetBuyerOrdersAsync(int buyerId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task SaveOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
