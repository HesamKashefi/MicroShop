namespace MicroShop.View.Models.DTOs
{
    public record OrderDto(int Id, DateTime CreatedAt, OrderStatusEnum Status);
}
