using EventBus.Core;

namespace Catalog.Application.Events
{
    public class ProductPriceUpdated : Event
    {
        public required string ProductId { get; init; }
        public required decimal NewPrice { get; init; }
    }
}
