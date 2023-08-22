using MediatR;
using Orders.Application.Models;
using Orders.Domain;
using Orders.Domain.Contracts;

namespace Orders.Application.Commands
{
    public record CreateOrderCommand(int BuyerId, Cart Cart, string Country, string City, string Street, string ZipCode) : IRequest;

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
    {
        private readonly IOrdersRepository _repository;

        public CreateOrderCommandHandler(IOrdersRepository repository)
        {
            _repository = repository;
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
                order.Add(new OrderItem(item.ProductId, item.ProductName, item.Quantity));
            }

            await _repository.SaveOrderAsync(order);
        }
    }
}
