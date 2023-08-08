using Common.Data;
using MicroShop.View.Models.DTOs;

namespace MicroShop.View.Models.HttpClients
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(HttpClient client, ILogger<CatalogService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<PagedResult<ProductDto[]>> GetProductsAsync(int page = 1)
        {
            try
            {
                var products = await _client.GetFromJsonAsync<PagedResult<ProductDto[]>>("products?page=" + page);
                _logger.LogTrace("Received Products {@Products}", new { Products = products });
                return products!;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get products");
                throw;
            }
        }
    }
}
