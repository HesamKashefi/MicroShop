using EventBus.Core;
using MediatR;
using Orders.Application.Commands;
using Orders.Application.Events;

namespace Orders.Application.EventHandlers
{
    public class UserCheckoutStartedEventHandler : IEventHandler<UserCheckoutStartedEvent>
    {
        private readonly IMediator _mediator;

        public UserCheckoutStartedEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(UserCheckoutStartedEvent @event)
        {
            await _mediator.Send(new CreateOrderCommand(@event.BuyerId, @event.BuyerName, @event.Cart, @event.Country, @event.City, @event.Street, @event.ZipCode));
        }
    }
}
