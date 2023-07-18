using StackExchange.Redis;
using System.Text.Json;

namespace Cart.Api.Services
{
    public class CartService : ICartService
    {
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        private readonly IDatabase _database;

        public CartService(IDatabase database)
        {
            _database = database;
        }

        public async Task UpdateCartAsync(string userName, Models.Cart cart)
        {
            var cartJson = JsonSerializer.Serialize(cart, _options);
            await _database.StringSetAsync(userName, cartJson);
        }

        public async Task DeleteCartAsync(string userName)
        {
            await _database.KeyDeleteAsync(userName);
        }

        public async Task<Models.Cart> GetCartAsync(string userName)
        {
            var cartJson = await _database.StringGetAsync(userName);
            if (cartJson.HasValue)
            {
                return JsonSerializer.Deserialize<Cart.Api.Models.Cart>(cartJson!, _options)!;
            }

            return new Models.Cart { CartItems = new() };
        }
    }
}
