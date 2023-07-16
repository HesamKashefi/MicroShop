namespace Catalog.Application.Models
{
    public class ProductDto
    {
        public required string Id { get; init; }
        public required string Name { get; init; }
        public decimal Price { get; init; }
        public string? ImageUrl { get; init; }
    }
}
