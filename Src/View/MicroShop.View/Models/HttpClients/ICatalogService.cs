using MicroShop.View.Models.DTOs;

namespace MicroShop.View.Models.HttpClients
{
    public interface ICatalogService
    {
        Task<ProductDto[]> GetProductsAsync();
    }
}
