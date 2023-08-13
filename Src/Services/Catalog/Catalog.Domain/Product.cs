using MongoDB.Bson;

namespace Catalog.Domain
{
    public class Product
    {
        public const string CollectionName = "Products";

        public ObjectId Id { get; set; }

        public required string Name { get; set; }
        public double Price { get; set; }
        public string? ImageFileName { get; set; }
    }
}
