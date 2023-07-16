using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace Cart.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        private readonly IDatabase _database;
        private readonly string _username = "hesam";

        public CartController(IDatabase database)
        {
            _database = database;
        }

        [HttpGet]
        public async Task<Models.Cart> Get()
        {
            var cart = await GetCart();
            return cart;
        }

        [HttpPut]
        public async Task<Models.Cart> UpdateCart([FromBody] Models.Cart cart)
        {
            var cartJson = JsonSerializer.Serialize(cart, _options);
            await _database.StringSetAsync(_username, cartJson);
            return cart;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCart()
        {
            await _database.KeyDeleteAsync(_username);
            return NoContent();
        }

        private async Task<Models.Cart> GetCart()
        {
            var cartJson = await _database.StringGetAsync(_username);
            if (cartJson.HasValue)
            {
                return JsonSerializer.Deserialize<Cart.Api.Models.Cart>(cartJson!, _options)!;
            }

            return new Models.Cart { CartItems = new() };
        }
    }
}
