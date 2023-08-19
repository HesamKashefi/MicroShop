namespace Orders.Domain
{
    public enum OrderStatusEnum : byte
    {
        Submitted = 0,
        Approved = 1,
        Shipping = 3,
        Shipped = 4,
        Cancelled = 5
    }
}
