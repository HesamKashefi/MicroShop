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

        public async Task<ProductDto[]> GetProductsAsync()
        {
            try
            {
                var products = await _client.GetFromJsonAsync<ProductDto[]>("products");
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
