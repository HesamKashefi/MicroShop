using EventBus.Core;
using Microsoft.AspNetCore.SignalR;
using Orders.SignalR.Hubs;

namespace Orders.SignalR.Events
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
        private readonly IHubContext<OrderingHub> _hub;
        private readonly ILogger<OrderStatusChangedToSubmittedEventHandler> _logger;

        public OrderStatusChangedToSubmittedEventHandler(
            IHubContext<OrderingHub> hub,
            ILogger<OrderStatusChangedToSubmittedEventHandler> logger)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(OrderStatusChangedToSubmittedEvent @event)
        {
            _logger.LogTrace("Handling Event {@Event}", @event);

            await _hub.Clients.Group(@event.BuyerName).SendAsync("Update", new { orderId = @event.OrderId });
        }
    }
}
