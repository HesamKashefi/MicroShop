using Catalog.Application.Commands;
using Catalog.Application.Models;
using Catalog.Application.Queries;
using Common.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateProductPriceCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return NoContent();
        }

        [HttpGet]
        public async Task<PagedResult<ProductDto[]>> GetAll([FromQuery] int page = 1)
        {
            return await _mediator.Send(new GetProductsQuery(page), HttpContext.RequestAborted);
        }

        [HttpGet("{id}")]
        public async Task<ProductDto?> Get(string id)
        {
            return await _mediator.Send(new GetProductByIdQuery(id), HttpContext.RequestAborted);
        }
    }
}
