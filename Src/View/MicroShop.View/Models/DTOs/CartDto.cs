namespace MicroShop.View.Models.DTOs
{
    public record CartDto(CartItemDto[] CartItems);
    public record CartItemDto(string ProductId, string ProductName, string ProductImageUrl, double ProductPrice, int Quantity);


    public record CartUpdateDto(CartItemUpdateDto[] CartItems);
    public class CartItemUpdateDto
    {
        public CartItemUpdateDto() { }

        public CartItemUpdateDto(string productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public string? ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
