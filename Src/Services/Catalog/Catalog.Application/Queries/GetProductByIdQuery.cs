using Catalog.Application.Models;
using Catalog.Domain;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Application.Queries
{
    public record GetProductByIdQuery(string Id) : IRequest<ProductDto?>;

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IMongoDatabase _database;
        private readonly IOptions<PictureFileSettings> _options;

        public GetProductByIdQueryHandler(IMongoDatabase database, IOptions<PictureFileSettings> options)
        {
            _database = database;
            _options = options;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var collection = _database.GetCollection<Product>(Product.CollectionName);
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
                ImageUrl = _options.Value.ImageBaseUrl.Replace("[0]", product.Id.ToString()),
                ImageFileName = product.ImageFileName,
                Price = product.Price
            };
        }
    }
}
