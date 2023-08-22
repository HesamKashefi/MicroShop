using Common.Data;
using Orders.Domain.Dtos;

namespace Orders.Domain.Contracts
{
    public interface IOrdersRepository
    {
        Task<Order?> GetOrderAsync(int orderId);
        Task<PagedResult<OrderDto[]>> GetBuyerOrdersAsync(int buyerId, int page = 1);
        Task SaveOrderAsync(Order order);
    }
}
