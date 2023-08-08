using Catalog.Application.Models;
using Catalog.Domain;
using Common.Data;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Catalog.Application.Queries
{
    public record GetProductsQuery(int Page) : IRequest<PagedResult<ProductDto[]>>;

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductDto[]>>
    {
        private readonly IMongoDatabase _db;

        public GetProductsQueryHandler(IMongoDatabase db)
        {
            _db = db;
        }

        public async Task<PagedResult<ProductDto[]>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var collection = _db.GetCollection<Product>("Products");
            var query = collection.AsQueryable().OrderBy(x => x.Id);
            var products = await query
                .Skip((request.Page - 1) * PagedResult.PageSize)
                .Take(PagedResult.PageSize)
                .ToListAsync();
            var dtos = products
                .Select(x => new ProductDto
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    Price = x.Price
                })
                .ToArray();

            var count = await query.CountAsync();

            return new PagedResult<ProductDto[]>
            {
                Data = dtos,
                TotalCount = count,
                CurrentPage = request.Page,
                TotalPages = (int)Math.Ceiling((double)count / PagedResult.PageSize)
            };
        }
    }
}
