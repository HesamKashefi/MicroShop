namespace Orders.Domain
{
    public class Order
    {
        public int BuyerId { get; init; }

        public DateTime CreatedAt { get; private init; } = DateTime.Now;
        public required Address Address { get; init; }

        private List<OrderItem> _orderItems = new();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    }
}