using EventBus.Core;

namespace Catalog.Application.Events
{
    public class ProductInfoUpdated : Event
    {
        public required string ProductId { get; init; }
        public required string NewName { get; init; }
    }
}
