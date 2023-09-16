using EventBus.Core;
using Microsoft.Extensions.Logging;
using Orders.Domain.Contracts;

namespace Orders.Application.Events
{
    public class OrderPaymentSucceedEvent : Event
    {
        public OrderPaymentSucceedEvent(int buyerId, string buyerName, int orderId)
        {
            BuyerId = buyerId;
            BuyerName = buyerName;
            OrderId = orderId;
        }

        public int BuyerId { get; }
        public string BuyerName { get; }
        public int OrderId { get; }
    }

    public class OrderPaymentSucceedEventHandler : IEventHandler<OrderPaymentSucceedEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IOrdersRepository _repository;
        private readonly ILogger<OrderPaymentSucceedEventHandler> _logger;

        public OrderPaymentSucceedEventHandler(
            IEventBus eventBus,
            IOrdersRepository repository,
            ILogger<OrderPaymentSucceedEventHandler> logger)
        {
            _eventBus = eventBus;
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(OrderPaymentSucceedEvent @event)
        {
            using var scope = _logger.BeginScope("Successful payment event order {@Event}", @event);

            var order = await _repository.GetOrderAsync(@event.OrderId);

            if (order is not null)
            {
                order.MarkAsPaid();
                await _repository.SaveOrderAsync(order);
                _logger.LogInformation("Setting successful payment order {@Order}", order);
                _eventBus.Publish(new OrderMarkedAsPaidEvent(@event.BuyerId, @event.BuyerName, @event.OrderId));
            }
        }
    }
}
