namespace MicroShop.View.Models.DTOs
{
    public record Address(string Country, string City, string Street, string ZipCode);
    public record OrderDetailsDto(int Id, int BuyerId, DateTime CreatedAt, bool IsPaid, Address Address, OrderStatusEnum Status, OrderItemDto[] OrderItems);
}
