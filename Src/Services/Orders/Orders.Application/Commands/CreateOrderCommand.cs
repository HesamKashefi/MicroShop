using EventBus.Core;
using MediatR;
using Orders.Application.Events;
using Orders.Application.Models;
using Orders.Domain;
using Orders.Domain.Contracts;

namespace Orders.Application.Commands
{
    public record CreateOrderCommand(int BuyerId, Cart Cart, string Country, string City, string Street, string ZipCode) : IRequest;

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
    {
        private readonly IOrdersRepository _repository;
        private readonly IEventBus _bus;

        public CreateOrderCommandHandler(IOrdersRepository repository, IEventBus bus)
        {
            _repository = repository;
            _bus = bus;
        }

        public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order()
            {
                BuyerId = request.BuyerId,
                Address = new Address
                {
                    City = request.City,
                    Country = request.Country,
                    Street = request.Street,
                    ZipCode = request.ZipCode
                }
            };
            foreach (var item in request.Cart.CartItems)
            {
                order.Add(new OrderItem(item.ProductId, item.ProductName, item.ProductImageUrl, item.ProductPrice, item.Quantity));
            }

            await _repository.SaveOrderAsync(order);
            _bus.Publish(new OrderStatusChangedToSubmittedEvent(order.BuyerId, order.Id));
        }
    }
}
