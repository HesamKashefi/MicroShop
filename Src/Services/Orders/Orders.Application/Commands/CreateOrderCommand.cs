using EventLog;
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
        private readonly IEventLogService _eventLogService;

        public CreateOrderCommandHandler(IOrdersRepository repository, IEventLogService eventLogService)
        {
            _repository = repository;
            _eventLogService = eventLogService;
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

            using var transaction = await _repository.BeginTransactionAsync();
            await _repository.SaveOrderAsync(order);
            await _eventLogService.AddEventAsync(new OrderStatusChangedToSubmittedEvent(order.BuyerId, order.Id), transaction, cancellationToken);
            await _eventLogService.PublishPendingEventsAsync(transaction, cancellationToken);
            await transaction.CommitAsync();
        }
    }
}
