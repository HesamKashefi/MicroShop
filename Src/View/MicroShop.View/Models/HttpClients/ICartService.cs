using MicroShop.View.Models.DTOs;

namespace MicroShop.View.Models.HttpClients
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync();
        Task UpdateCartAsync(CartUpdateDto cart);
        Task AddItemAsync(string productId);
        Task RemoveItemAsync(string productId);
        Task Checkout(CartCheckoutDto dto);
    }
}
