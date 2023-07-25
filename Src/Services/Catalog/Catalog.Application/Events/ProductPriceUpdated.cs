using EventBus.Core;

namespace Catalog.Application.Events
{
    public class ProductPriceUpdated : Event
    {
        public required string ProductId { get; init; }
        public required double NewPrice { get; init; }
    }
}
