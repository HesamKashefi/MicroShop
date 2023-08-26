using Common.Data;
using MicroShop.View.Models.DTOs;

namespace MicroShop.View.Models.HttpClients
{
    public interface IOrdersService
    {
        Task<PagedResult<OrderDto[]>> GetOrdersAsync(int page);
    }
}
