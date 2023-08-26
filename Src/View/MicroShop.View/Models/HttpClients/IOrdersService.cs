using Common.Data;
using MicroShop.View.Models.DTOs;

namespace MicroShop.View.Models.HttpClients
{
    public interface IOrdersService
    {
        Task<Result<OrderDetailsDto>> GetOrderAsync(int OrderId);
        Task<PagedResult<OrderDto[]>> GetOrdersAsync(int page);
    }
}
