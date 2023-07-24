using Catalog.Domain;
using Grpc.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using static Catalog.Api.Protos.CatalogService;

namespace Catalog.Api.Protos
{
    public class MyCatalogService : CatalogServiceBase
    {
        private readonly IMongoDatabase _database;

        public MyCatalogService(IMongoDatabase database)
        {
            _database = database;
        }

        public override async Task<ProductDto?> GetProductById(GetProduct request, ServerCallContext context)
        {
            var collection = _database.GetCollection<Product>("Products");
            var product = await collection.Find(Builders<Product>.Filter.Eq(x => x.Id, ObjectId.Parse(request.Id))).FirstOrDefaultAsync();

            if (product is null) return null;

            return new ProductDto
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Price = (int)product.Price,
                ImageUrl = product.ImageUrl
            };
        }
    }
}
