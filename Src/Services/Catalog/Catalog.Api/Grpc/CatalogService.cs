using Catalog.Api.Protos;
using Catalog.Domain;
using Grpc.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using static Catalog.Api.Protos.CatalogService;

namespace Catalog.Api.Grpc
{
    public class MyCatalogService : CatalogServiceBase
    {
        private readonly IMongoDatabase _database;

        public MyCatalogService(IMongoDatabase database)
        {
            _database = database;
        }

        public override async Task<GetProductsByIdsResponse?> GetProductsById(GetProductsByIdsQuery request, ServerCallContext context)
        {
            var collection = _database.GetCollection<Product>("Products");
            var ids = request.Ids.Select(x => ObjectId.Parse(x));
            var products = await collection.Find(Builders<Product>.Filter.In(x => x.Id, ids)).ToListAsync();
            var data = products.Select(product => new ProductDto
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl
            });

            var productsReponse = new GetProductsByIdsResponse();
            foreach (var item in data)
            {
                productsReponse.Products.Add(item);
            }
            return productsReponse;
        }
    }
}
