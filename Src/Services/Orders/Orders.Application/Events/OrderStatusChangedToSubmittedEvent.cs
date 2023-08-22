using EventBus.Core;

namespace Orders.Application.Events
{
    public class OrderStatusChangedToSubmittedEvent : Event
    {
        public OrderStatusChangedToSubmittedEvent(int buyerId, int orderId)
        {
            BuyerId = buyerId;
            OrderId = orderId;
        }

        public int BuyerId { get; }
        public int OrderId { get; }
    }
}
