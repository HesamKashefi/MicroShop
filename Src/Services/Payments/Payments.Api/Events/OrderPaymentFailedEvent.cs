using EventBus.Core;

namespace Payments.Api.Events
{
    public class OrderPaymentFailedEvent : Event
    {
        public OrderPaymentFailedEvent(int orderId)
        {
            OrderId = orderId;
        }
        public int OrderId { get; }
    }
}
