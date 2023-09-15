using EventBus.Core;

namespace Payments.Api.Events
{
    public class OrderPaymentSucceedEvent : Event
    {
        public OrderPaymentSucceedEvent(int orderId)
        {
            OrderId = orderId;
        }
        public int OrderId { get; }
    }
}
