using Common.Data;
using Common.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Models;
using Orders.Domain;
using Orders.Domain.Contracts;

namespace Orders.Application.Queries
{
    public partial class OrdersController : BaseController
    {
        [HttpGet("{orderId}")]
        public async Task<Result<OrderDetailsDto>> GetBuyerOrders(int orderId)
        {
            return await Mediator.Send(new GetOrderDetailsQuery(orderId), HttpContext.RequestAborted);
        }
    }

    public record GetOrderDetailsQuery(int OrderId) : IRequest<Result<OrderDetailsDto>>;

    public class GetOrderDetailsQueryHandler : IRequestHandler<GetOrderDetailsQuery, Result<OrderDetailsDto>>
    {
        private readonly IUserService _userService;
        private readonly IOrdersRepository _repository;

        public GetOrderDetailsQueryHandler(
            IUserService userService,
            IOrdersRepository repository)
        {
            _userService = userService;
            _repository = repository;
        }

        public async Task<Result<OrderDetailsDto>> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetOrderAsync(request.OrderId);

            if (order is null)
            {
                return Result.NotFound<OrderDetailsDto>();
            }

            var userId = _userService.GetId();

            if (order.BuyerId != userId && !_userService.IsAdmin())
            {
                return Result.Forbidden<OrderDetailsDto>();
            }

            return Result.Success(new OrderDetailsDto
            {
                Id = order.Id,
                BuyerId = order.BuyerId,
                Address = order.Address,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                OrderItems = order.OrderItems.ToList()
            });
        }
    }

    public class OrderDetailsDto
    {
        public int Id { get; init; }

        public int BuyerId { get; init; }
        public string? ProductImageUrl { get; init; }
        public double ProductPrice { get; init; }

        public DateTime CreatedAt { get; init; }
        public required Address Address { get; init; }

        public OrderStatusEnum Status { get; init; }

        public required List<OrderItem> OrderItems { get; init; }
    }
}
