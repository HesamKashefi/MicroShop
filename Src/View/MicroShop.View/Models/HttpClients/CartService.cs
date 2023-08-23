using MicroShop.View.Models.DTOs;

namespace MicroShop.View.Models.HttpClients
{
    public class CartService : ICartService
    {
        private readonly HttpClient _client;
        private readonly ILogger<CatalogService> _logger;

        public CartService(HttpClient client, ILogger<CatalogService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<CartDto> GetCartAsync()
        {
            return await _client.GetFromJsonAsync<CartDto>("cart") ?? new CartDto(Array.Empty<CartItemDto>());
        }

        public async Task UpdateCartAsync(CartUpdateDto cart)
        {
            var response = await _client.PutAsJsonAsync("api/v1/Cart", cart);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddItemAsync(string productId)
        {
            try
            {
                var currentCart = await GetCartAsync();

                var cartItems = currentCart.CartItems.Select(x => new CartItemUpdateDto(x.ProductId, x.Quantity)).ToList();
                var cartItem = cartItems.FirstOrDefault(x => x.ProductId == productId);
                if (cartItem is not null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    cartItems.Add(new CartItemUpdateDto(productId, 1));
                }

                var cartUpdate = new CartUpdateDto(cartItems.ToArray());
                await UpdateCartAsync(cartUpdate);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not update cart");
                throw;
            }
        }

        public async Task RemoveItemAsync(string productId)
        {
            try
            {
                var currentCart = await GetCartAsync();

                var cartItems = currentCart.CartItems.Select(x => new CartItemUpdateDto(x.ProductId, x.Quantity)).ToList();
                var cartItem = cartItems.FirstOrDefault(x => x.ProductId == productId);
                if (cartItem is not null)
                {
                    cartItem.Quantity--;
                    if (cartItem.Quantity < 1)
                    {
                        cartItems.Remove(cartItem);
                    }
                }

                var cartUpdate = new CartUpdateDto(cartItems.ToArray());
                await UpdateCartAsync(cartUpdate);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not update cart");
                throw;
            }
        }

        public async Task Checkout(CartCheckoutDto dto)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("Cart/Checkout", dto);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not update cart");
                throw;
            }
        }
    }
}
