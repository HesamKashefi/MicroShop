using Cart.Api.Services;
using EventBus.Core;
using StackExchange.Redis;

namespace Cart.Api.Events
{
    public class ProductPriceUpdated : Event
    {
        public required string ProductId { get; init; }
        public required double NewPrice { get; init; }
    }

    public class ProductPriceUpdatedHandler : IEventHandler<ProductPriceUpdated>
    {
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ICartService _cartService;
        private readonly ILogger<ProductPriceUpdatedHandler> _logger;

        public ProductPriceUpdatedHandler(
            IConnectionMultiplexer connectionMultiplexer,
            ICartService cartService,
            ILogger<ProductPriceUpdatedHandler> logger)
        {
            _connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
            _cartService = cartService;
            _logger = logger;
            _database = _connectionMultiplexer.GetDatabase();
        }

        public async Task Handle(ProductPriceUpdated @event)
        {
            _logger.LogTrace("Event Received, {@Event}", @event);
            await foreach (var key in _connectionMultiplexer.GetServers()[0].KeysAsync())
            {
                var cart = await _cartService.GetCartAsync(key!);
                if (cart.CartItems.Any(x => x.ProductId == @event.ProductId))
                {
                    var newCart = new Models.Cart
                    {
                        CartItems = cart.CartItems.Select(x =>
                        {
                            if (x.ProductId == @event.ProductId)
                            {
                                return x with { ProductPrice = @event.NewPrice };
                            }
                            return x;
                        }).ToList()
                    };
                    await _cartService.UpdateCartAsync(key!, newCart);
                }
            }
            await Task.CompletedTask;
        }
    }
}
