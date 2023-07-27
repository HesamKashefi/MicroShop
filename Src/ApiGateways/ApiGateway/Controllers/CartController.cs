using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Catalog.Api.Protos.CatalogService;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "OpenIddict.Validation.AspNetCore")]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CatalogServiceClient _catalogService;

        public CartController(CatalogServiceClient catalogService)
        {
            _catalogService = catalogService;
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

            return NoContent();
        }

        public record Cart(CartItem[] CartItems);
        public record CartItem([Required(AllowEmptyStrings = false)] string ProductId, [Range(1, short.MaxValue)] short Quantity);
    }
}
