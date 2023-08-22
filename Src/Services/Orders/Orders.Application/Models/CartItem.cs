namespace Orders.Application.Models
{
    public record CartItem(string ProductId, string ProductName, string ProductImageUrl, double ProductPrice, int Quantity);
}
