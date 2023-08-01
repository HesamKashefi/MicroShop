using MicroShop.View.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroShop.View.Pages
{
    [Authorize]
    public class CartModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CartModel> _logger;

        public CartDto? Cart { get; private set; }

        [BindProperty]
        public CartItemUpdateDto CartItem { get; set; }

        public CartModel(HttpClient httpClient, ILogger<CartModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            try
            {
                Cart = await _httpClient.GetFromJsonAsync<CartDto>("/Cart");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not load cart");
            }
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            try
            {
                Cart = await _httpClient.GetFromJsonAsync<CartDto>("/Cart") ?? new CartDto(Array.Empty<CartItemDto>());

                var cartItems = Cart.CartItems.Select(x => new CartItemUpdateDto(x.ProductId, x.Quantity)).ToList();
                var cartItem = cartItems.FirstOrDefault(x => x.ProductId == CartItem.ProductId);
                if (cartItem is not null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    cartItems.Add(new CartItemUpdateDto(CartItem.ProductId, 1));
                }

                var cartUpdate = new CartUpdateDto(cartItems.ToArray());
                var response = await _httpClient.PutAsJsonAsync("api/v1/Cart", cartUpdate);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not update cart");
            }
            return RedirectToPage("Cart");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            try
            {
                Cart = await _httpClient.GetFromJsonAsync<CartDto>("/Cart") ?? new CartDto(Array.Empty<CartItemDto>());

                var cartItems = Cart.CartItems.Select(x => new CartItemUpdateDto(x.ProductId, x.Quantity)).ToList();
                var cartItem = cartItems.FirstOrDefault(x => x.ProductId == CartItem.ProductId);
                if (cartItem is not null)
                {
                    cartItem.Quantity--;
                    if (cartItem.Quantity < 1)
                    {
                        cartItems.Remove(cartItem);
                    }
                }

                var cartUpdate = new CartUpdateDto(cartItems.ToArray());

                var response = await _httpClient.PutAsJsonAsync("api/v1/Cart", cartUpdate);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not update cart");
            }
            return RedirectToPage("Cart");
        }
    }
}
