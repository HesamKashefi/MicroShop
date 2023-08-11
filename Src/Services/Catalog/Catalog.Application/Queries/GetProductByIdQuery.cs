using Catalog.Application.Models;
using Catalog.Domain;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Application.Queries
{
    public record GetProductByIdQuery(string Id) : IRequest<ProductDto?>;

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IMongoDatabase _database;

        public GetProductByIdQueryHandler(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var collection = _database.GetCollection<Product>("Products");
            var filter = Builders<Product>.Filter.Eq(x => x.Id, ObjectId.Parse(request.Id));
            var product = await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return null;
            }

            return new ProductDto
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price
            };
        }
    }
}
