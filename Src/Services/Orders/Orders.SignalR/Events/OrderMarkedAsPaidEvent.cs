using EventBus.Core;
using Microsoft.AspNetCore.SignalR;
using Orders.SignalR.Hubs;

namespace Orders.SignalR.Events
{
    public class OrderMarkedAsPaidEvent : Event
    {
        public OrderMarkedAsPaidEvent(int buyerId, string buyerName, int orderId)
        {
            BuyerId = buyerId;
            BuyerName = buyerName;
            OrderId = orderId;
        }

        public int BuyerId { get; }
        public string BuyerName { get; }
        public int OrderId { get; }
    }

    public class OrderMarkedAsPaidEventHandler : IEventHandler<OrderMarkedAsPaidEvent>
    {
        private readonly IHubContext<OrderingHub> _hub;
        private readonly ILogger<OrderMarkedAsPaidEventHandler> _logger;

        public OrderMarkedAsPaidEventHandler(
            IHubContext<OrderingHub> hub,
            ILogger<OrderMarkedAsPaidEventHandler> logger)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(OrderMarkedAsPaidEvent @event)
        {
            _logger.LogTrace("Handling Event {@Event}", @event);

            await _hub.Clients.Group(@event.BuyerName).SendAsync("Update", new { orderId = @event.OrderId });
        }
    }
}
