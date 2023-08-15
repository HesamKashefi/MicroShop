namespace Orders.Domain
{
    public record OrderItem(string ProductId, string ProductName, int Quantity);
}