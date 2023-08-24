namespace Orders.Domain
{
    public enum OrderStatusEnum : byte
    {
        Submitted = 0,
        Approved = 1,
        Shipped = 2,
        Cancelled = 10
    }
}
