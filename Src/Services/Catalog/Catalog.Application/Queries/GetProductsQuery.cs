using Catalog.Application.Models;
using Catalog.Domain;
using MediatR;
using MongoDB.Driver;

namespace Catalog.Application.Queries
{
    public record GetProductsQuery() : IRequest<ProductDto[]>;

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ProductDto[]>
    {
        private readonly IMongoDatabase _db;

        public GetProductsQueryHandler(IMongoDatabase db)
        {
            _db = db;
        }

        public async Task<ProductDto[]> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var collection = _db.GetCollection<Product>("Products");
            var products = await collection.AsQueryable().ToListAsync();
            var dtos = products
                .Select(x => new ProductDto
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    Price = x.Price
                })
                .ToArray();

            return dtos;
        }
    }
}
