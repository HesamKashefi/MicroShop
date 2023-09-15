using EventBus.Core;
using Microsoft.Extensions.Options;
using Payments.Api.Models;

namespace Payments.Api.Events
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
        private readonly IOptions<PaymentsSettings> _options;
        private readonly IEventBus _bus;

        public OrderStatusChangedToSubmittedEventHandler(IOptions<PaymentsSettings> options, IEventBus bus)
        {
            _options = options;
            _bus = bus;
        }

        public Task Handle(OrderStatusChangedToSubmittedEvent @event)
        {
            // Will immediately set a fake result for any order
            if (_options.Value.PaymentsMustSucceed)
            {
                _bus.Publish(new OrderPaymentSucceedEvent(@event.OrderId));
            }
            else
            {
                _bus.Publish(new OrderPaymentFailedEvent(@event.OrderId));
            }

            return Task.CompletedTask;
        }
    }
}
