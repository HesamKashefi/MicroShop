using Cart.Api.Events;
using Cart.Api.Models;
using Cart.Api.Services;
using Common.Services;
using EventBus.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cart.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IEventBus _bus;
        private readonly IUserService _userService;

        public CartController(ICartService cartService, IEventBus bus, IUserService userService)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public async Task<Models.Cart> Get()
        {
            var cart = await _cartService.GetCartAsync(_userService.GetName());
            return cart;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateCart([FromBody] Models.Cart cart)
        {
            await _cartService.UpdateCartAsync(_userService.GetName(), cart);
            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCart()
        {
            await _cartService.DeleteCartAsync(_userService.GetName());
            return NoContent();
        }

        [HttpPost("Checkout")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> Checkout([FromBody] CartCheckout cartCheckout)
        {
            var cart = await _cartService.GetCartAsync(_userService.GetName());

            var userCheckoutStartedEvent = new UserCheckoutStartedEvent(_userService.GetId(), User.FindFirstValue("name")!, cart, cartCheckout.Country, cartCheckout.City, cartCheckout.Street, cartCheckout.ZipCode);

            _bus.Publish(userCheckoutStartedEvent);

            return Accepted();
        }
    }
}
