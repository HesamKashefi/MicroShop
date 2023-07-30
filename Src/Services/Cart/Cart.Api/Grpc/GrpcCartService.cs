using Cart.Api.Protos;
using Cart.Api.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Cart.Api.Grpc
{
    [Authorize]
    public class GrpcCartService : Protos.CartService.CartServiceBase
    {
        private readonly ICartService _cartService;

        public GrpcCartService(ICartService cartService)
        {
            _cartService = cartService;
        }

        public override async Task<UpdateCartResponse> UpdateCart(UpdateCartCommand request, ServerCallContext context)
        {
            var userName = context.GetHttpContext().User.Identity!.Name;
            await _cartService.UpdateCartAsync(userName!, new Models.Cart
            {
                CartItems = request.CartItems.Select(x => new Models.CartItem(x.ProductId, x.ProductName, x.ProductImageUrl, x.ProductPrice, x.Quantity)).ToList()
            });

            return new UpdateCartResponse
            {
                Ok = true
            };
        }
    }
}
