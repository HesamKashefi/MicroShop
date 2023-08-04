using MicroShop.View.Models.DTOs;
using MicroShop.View.Models.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroShop.View.Pages
{
    [Authorize]
    public class CartModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartModel> _logger;

        public CartDto? Cart { get; private set; }

        [BindProperty]
        public CartItemUpdateDto CartItem { get; set; }

        public CartModel(ICartService cartService, ILogger<CartModel> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            try
            {
                Cart = await _cartService.GetCartAsync();
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
                await _cartService.AddItemAsync(CartItem.ProductId);
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
                await _cartService.RemoveItemAsync(CartItem.ProductId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not update cart");
            }
            return RedirectToPage("Cart");
        }
    }
}
