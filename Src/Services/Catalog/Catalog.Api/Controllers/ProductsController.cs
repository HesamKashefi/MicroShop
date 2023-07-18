using Catalog.Application.Commands;
using Catalog.Application.Models;
using Catalog.Application.Queries;
using MediatR;
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
        public async Task<IActionResult> Update([FromBody] UpdateProductPriceCommand command)
        {
            await _mediator.Send(command, HttpContext.RequestAborted);
            return NoContent();
        }

        [HttpGet]
        public async Task<ProductDto[]> GetAll()
        {
            return await _mediator.Send(new GetProductsQuery(), HttpContext.RequestAborted);
        }
    }
}
