using MicroShop.View.Models.DTOs;
using MicroShop.View.Models.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroShop.View.Pages.Cart
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CheckoutModel> _logger;

        public CartDto? Cart { get; private set; }

        [BindProperty]
        public CartCheckoutDto? CartCheckoutDto { get; set; }

        public CheckoutModel(ICartService cartService, ILogger<CheckoutModel> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Cart = await _cartService.GetCartAsync();
                if (Cart?.CartItems.Length == 0)
                {
                    return RedirectToPage("Index");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not load cart");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _cartService.Checkout(CartCheckoutDto!);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }

            return RedirectToPage("/Orders/Index");
        }
    }
}
