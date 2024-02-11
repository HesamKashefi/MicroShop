using Catalog.Application.Events;
using Catalog.Domain;
using EventBus.Core;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Application.Commands
{
    public record UpdateProductInfoCommand(string ProductId, string NewName) : IRequest;

    public class UpdateProductInfoCommandHandler(IMongoDatabase database, IEventBus eventBus) : IRequestHandler<UpdateProductInfoCommand>
    {
        private readonly IMongoDatabase _database = database;
        private readonly IEventBus _eventBus = eventBus;

        public async Task Handle(UpdateProductInfoCommand request, CancellationToken cancellationToken)
        {
            var collection = _database.GetCollection<Product>("Products");
            var filter = Builders<Product>.Filter.Eq(x => x.Id, ObjectId.Parse(request.ProductId));
            var update = Builders<Product>.Update.Set(x => x.Name, request.NewName);
            await collection.UpdateOneAsync(filter, update);
            _eventBus.Publish(new ProductInfoUpdated { ProductId = request.ProductId, NewName = request.NewName });
        }
    }
}
