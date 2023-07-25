using Catalog.Application.Events;
using Catalog.Domain;
using EventBus.Core;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Application.Commands
{
    public record UpdateProductPriceCommand(string ProductId, double NewPrice) : IRequest;

    public class UpdateProductPriceCommandHandler : IRequestHandler<UpdateProductPriceCommand>
    {
        private readonly IMongoDatabase _database;
        private readonly IEventBus _eventBus;

        public UpdateProductPriceCommandHandler(IMongoDatabase database, IEventBus eventBus)
        {
            _database = database;
            _eventBus = eventBus;
        }

        public async Task Handle(UpdateProductPriceCommand request, CancellationToken cancellationToken)
        {
            var collection = _database.GetCollection<Product>("Products");
            var filter = Builders<Product>.Filter.Eq(x => x.Id, ObjectId.Parse(request.ProductId));
            var update = Builders<Product>.Update.Set(x => x.Price, request.NewPrice);
            await collection.UpdateOneAsync(filter, update);
            _eventBus.Publish(new ProductPriceUpdated { ProductId = request.ProductId, NewPrice = request.NewPrice });
        }
    }
}
