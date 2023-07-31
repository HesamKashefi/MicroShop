namespace MicroShop.View.Models
{
    public class ProductDto
    {
        public required string Id { get; init; }
        public required string Name { get; init; }
        public double Price { get; init; }
        public string? ImageUrl { get; init; }
    }
}
