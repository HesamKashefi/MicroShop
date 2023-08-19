namespace Orders.Domain.Contracts
{
    public interface IOrdersRepository
    {
        Task<Order?> GetOrderAsync(int orderId);
        Task<Order[]> GetBuyerOrdersAsync(int buyerId);
        Task SaveOrderAsync(Order order);
    }
}
