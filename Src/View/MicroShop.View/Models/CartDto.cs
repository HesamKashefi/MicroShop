namespace MicroShop.View.Models
{
    public record CartDto(CartItemDto[] CartItems);
    public record CartItemDto(string ProductId, string ProductName, string ImageUrl, double ProductPrice, int Quantity);


    public record CartUpdateDto(CartItemUpdateDto[] CartItems);
    public record CartItemUpdateDto(string ProductId, int Quantity);
}
