using MicroShop.View.Models.DTOs;
using MicroShop.View.Models.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MicroShop.View.Pages.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly ILogger<IndexModel> _logger;

        public CartDto? Cart { get; private set; }

        [BindProperty, Required(AllowEmptyStrings = false)]
        public string? ProductId { get; set; }

        public IndexModel(ICartService cartService, ILogger<IndexModel> logger)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _cartService.AddItemAsync(ProductId!);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not update cart");
            }
            return RedirectToPage("/Cart/Index");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _cartService.RemoveItemAsync(ProductId!);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not update cart");
            }
            return RedirectToPage("/Cart/Index");
        }
    }
}
