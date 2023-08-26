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
        [HttpGet("All")]
        public async Task<PagedResult<OrderDto[]>> GetAllOrders([FromQuery] int page = 1)
        {
            return await Mediator.Send(new GetAllOrdersQuery(page), HttpContext.RequestAborted);
        }
    }

    public record GetAllOrdersQuery(int Page) : IRequest<PagedResult<OrderDto[]>>;

    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, PagedResult<OrderDto[]>>
    {
        private readonly IUserService _userService;
        private readonly IOrdersRepository _repository;

        public GetAllOrdersQueryHandler(
            IUserService userService,
            IOrdersRepository repository)
        {
            _userService = userService;
            _repository = repository;
        }

        public async Task<PagedResult<OrderDto[]>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            if (!_userService.IsAdmin()) return PagedResult<OrderDto[]>.Forbidden();

            return await _repository.GetAllOrdersAsync(request.Page);
        }
    }
}
