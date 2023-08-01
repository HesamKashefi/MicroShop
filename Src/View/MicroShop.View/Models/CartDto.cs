namespace MicroShop.View.Models
{
    public record CartDto(CartItemDto[] CartItems);
    public record CartItemDto(string ProductId, string ProductName, string ImageUrl, double ProductPrice, int Quantity);


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
