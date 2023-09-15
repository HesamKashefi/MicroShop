using EventBus.Core;

namespace Orders.Application.Events
{
    public class OrderPaymentFailedEvent : Event
    {
        public OrderPaymentFailedEvent(int orderId)
        {
            OrderId = orderId;
        }
        public int OrderId { get; }
    }

    public class OrderPaymentFailedEventHandler : IEventHandler<OrderPaymentFailedEvent>
    {
        public Task Handle(OrderPaymentFailedEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
