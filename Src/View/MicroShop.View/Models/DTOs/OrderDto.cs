namespace MicroShop.View.Models.DTOs
{
    public record OrderDto(int Id, DateTime CreatedAt, OrderStatusEnum Status);
    public enum OrderStatusEnum : byte
    {
        Submitted = 0,
        Approved = 1,
        Shipped = 2,
        Cancelled = 10
    }
}
