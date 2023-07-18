using Cart.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cart.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<Models.Cart> Get()
        {
            var cart = await _cartService.GetCartAsync(User.Identity!.Name!);
            return cart;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCart([FromBody] Models.Cart cart)
        {
            await _cartService.UpdateCartAsync(User.Identity!.Name!, cart);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCart()
        {
            await _cartService.DeleteCartAsync(User.Identity!.Name!);
            return NoContent();
        }
    }
}
