namespace Cart.Api.Services
{
    public interface ICartService
    {

        Task UpdateCartAsync(string userName, Models.Cart cart);

        Task DeleteCartAsync(string userName);

        Task<Models.Cart> GetCartAsync(string userName);
    }
}
