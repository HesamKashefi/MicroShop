using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Cart.Api.Protos.CartService;
using static Catalog.Api.Protos.CatalogService;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CatalogServiceClient _catalogService;
        private readonly CartServiceClient _cartServiceClient;

        public CartController(CatalogServiceClient catalogService, CartServiceClient cartServiceClient)
        {
            _catalogService = catalogService;
            _cartServiceClient = cartServiceClient;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCart([FromBody] Cart cart)
        {
            var query = new Catalog.Api.Protos.GetProductsByIdsQuery();
            foreach (var id in cart.CartItems.Select(x => x.ProductId))
            {
                query.Ids.Add(id);
            }
            var productsResponse = await _catalogService.GetProductsByIdAsync(query);

            var updateCartCommand = new global::Cart.Api.Protos.UpdateCartCommand();
            foreach (var item in cart.CartItems)
            {
                var product = productsResponse.Products.SingleOrDefault(x => x.Id == item.ProductId);
                if (product is not null)
                {
                    updateCartCommand.CartItems.Add(new global::Cart.Api.Protos.CartItem()
                    {
                        ProductId = item.ProductId,
                        ProductImageUrl = product.ImageUrl,
                        ProductName = product.Name,
                        ProductPrice = product.Price,
                        Quantity = item.Quantity
                    });
                }
            }
            var cartUpdateResponse = await _cartServiceClient.UpdateCartAsync(updateCartCommand);

            return NoContent();
        }

        public record Cart(CartItem[] CartItems);
        public record CartItem([Required(AllowEmptyStrings = false)] string ProductId, [Range(1, short.MaxValue)] short Quantity);
    }
}
