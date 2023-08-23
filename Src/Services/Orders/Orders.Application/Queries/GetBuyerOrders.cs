using Common.Data;
using Common.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Models;
using Orders.Domain.Contracts;
using Orders.Domain.Dtos;

namespace Orders.Application.Queries
{
    public partial class OrdersController : BaseController
    {
        [HttpGet]
        public async Task<PagedResult<OrderDto[]>> GetBuyerOrders([FromServices] IUserService userService)
        {
            var userId = userService.GetId();
            return await Mediator.Send(new GetBuyerOrdersQuery(userId), HttpContext.RequestAborted);
        }
    }

    public record GetBuyerOrdersQuery(int BuyerId) : IRequest<PagedResult<OrderDto[]>>;

    public class GetBuyerOrdersQueryHandler : IRequestHandler<GetBuyerOrdersQuery, PagedResult<OrderDto[]>>
    {
        private readonly IOrdersRepository _repository;

        public GetBuyerOrdersQueryHandler(IOrdersRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<OrderDto[]>> Handle(GetBuyerOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetBuyerOrdersAsync(request.BuyerId);
        }
    }
}
