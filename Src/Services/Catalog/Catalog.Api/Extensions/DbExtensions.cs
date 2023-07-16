using Catalog.Domain;
using MongoDB.Driver;

namespace Catalog.Api.Extensions
{
    public static class DbExtensions
    {
        public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
            var collection = db.GetCollection<Product>("Products");

            if (!await collection.AsQueryable().AnyAsync())
            {
                await collection.InsertManyAsync(new[]
                {
                    new Product{ Name = "Product 1", Price = 2000, ImageUrl = "/Images/Product1.png" },
                    new Product{ Name = "Product 2", Price = 1000, ImageUrl = "/Images/Product2.png" },
                    new Product{ Name = "Product 3", Price = 1800, ImageUrl = "/Images/Product3.png" },
                    new Product{ Name = "Product 4", Price = 1200, ImageUrl = "/Images/Product4.png" },
                    new Product{ Name = "Product 5", Price = 1500, ImageUrl = "/Images/Product5.png" }
                });
            }
        }
    }
}
