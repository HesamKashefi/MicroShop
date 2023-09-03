using EventBus.Core;

namespace Orders.Application.Events
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
}
