using Cart.Api.Services;
using EventBus.Core;

namespace Cart.Api.Events
{
    public class OrderStatusChangedToSubmittedEvent : Event
    {
        public OrderStatusChangedToSubmittedEvent(int buyerId, string buyerName, int orderId)
        {
            BuyerId = buyerId;
            BuyerName = buyerName;
            OrderId = orderId;
        }

        public int BuyerId { get; }
        public string BuyerName { get; }
        public int OrderId { get; }
    }

    public class OrderStatusChangedToSubmittedEventHandler : IEventHandler<OrderStatusChangedToSubmittedEvent>
    {
        private readonly ICartService _cartService;

        public OrderStatusChangedToSubmittedEventHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task Handle(OrderStatusChangedToSubmittedEvent @event)
        {
            await _cartService.DeleteCartAsync(@event.BuyerName);
        }
    }
}
