using Catalog.Api.Protos;
using Catalog.Application.Models;
using Catalog.Domain;
using Grpc.Core;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using static Catalog.Api.Protos.CatalogService;

namespace Catalog.Api.Grpc
{
    public class MyCatalogService : CatalogServiceBase
    {
        private readonly IMongoDatabase _database;
        private readonly IOptions<PictureFileSettings> _options;

        public MyCatalogService(IMongoDatabase database, IOptions<PictureFileSettings> options)
        {
            _database = database;
            _options = options;
        }

        public override async Task<GetProductsByIdsResponse?> GetProductsById(GetProductsByIdsQuery request, ServerCallContext context)
        {
            var collection = _database.GetCollection<Product>(Product.CollectionName);
            var ids = request.Ids.Select(ObjectId.Parse);
            var products = await collection.Find(Builders<Product>.Filter.In(x => x.Id, ids)).ToListAsync();
            var data = products.Select(product => new Protos.ProductDto
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Price = product.Price,
                ImageUrl = _options.Value.ImageBaseUrl.Replace("[0]", product.Id.ToString())
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
