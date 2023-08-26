namespace MicroShop.View.Models.DTOs
{
    public record OrderItemDto(string ProductId, string ProductName, string ProductImageUrl, double ProductPrice, int Quantity);
}
