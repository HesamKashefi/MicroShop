using Common.Data;
using MicroShop.View.Models.DTOs;

namespace MicroShop.View.Models.HttpClients
{
    public class OrdersService : IOrdersService
    {
        private readonly HttpClient _client;
        private readonly ILogger<OrdersService> _logger;

        public OrdersService(HttpClient client, ILogger<OrdersService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<Result<OrderDetailsDto>> GetOrderAsync(int OrderId)
        {
            var data = await _client.GetFromJsonAsync<Result<OrderDetailsDto>>("Orders/" + OrderId);
            _logger.LogTrace("Order {OrderId} Received {@Order}", OrderId, data!);
            return data!;
        }

        public async Task<PagedResult<OrderDto[]>> GetOrdersAsync(int page)
        {
            var data = await _client.GetFromJsonAsync<PagedResult<OrderDto[]>>("Orders?page=" + page);
            _logger.LogTrace("Orders Received {@Orders}", data!);
            return data!;
        }
    }
}
