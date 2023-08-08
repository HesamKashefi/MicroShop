using Common.Data;
using MicroShop.View.Models.DTOs;

namespace MicroShop.View.Models.HttpClients
{
    public interface ICatalogService
    {
        Task<PagedResult<ProductDto[]>> GetProductsAsync(int page = 1);
    }
}
