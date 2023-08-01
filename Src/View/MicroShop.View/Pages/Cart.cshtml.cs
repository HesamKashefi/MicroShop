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
                Cart = await _httpClient.GetFromJsonAsync<CartDto>("/Cart");
                var cartItem = Cart.CartItems.FirstOrDefault(x => x.ProductId == CartItem.ProductId);

                var list = new List<CartItemUpdateDto>();

                if (cartItem is null)
                {
                    list.Add(new CartItemUpdateDto(this.CartItem.ProductId, 1));
                }
                else
                {
                    list.Add(new CartItemUpdateDto(this.CartItem.ProductId, cartItem.Quantity + 1));
                }
                list.AddRange(Cart.CartItems.Where(x => x.ProductId != CartItem.ProductId).Select(x => new CartItemUpdateDto(x.ProductId, x.Quantity)));

                var cartUpdate = new CartUpdateDto(list.ToArray());
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
                Cart = await _httpClient.GetFromJsonAsync<CartDto>("/Cart");
                var cartItem = Cart.CartItems.FirstOrDefault(x => x.ProductId == CartItem.ProductId);

                var list = new List<CartItemUpdateDto>();

                if (cartItem is not null)
                {
                    if (cartItem.Quantity > 1)
                    {
                        list.Add(new CartItemUpdateDto(this.CartItem.ProductId, cartItem.Quantity - 1));
                    }
                }
                list.AddRange(Cart.CartItems.Where(x => x.ProductId != CartItem.ProductId).Select(x => new CartItemUpdateDto(x.ProductId, x.Quantity)));

                var cartUpdate = new CartUpdateDto(list.ToArray());
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
