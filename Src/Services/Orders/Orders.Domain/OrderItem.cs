namespace Orders.Domain
{
    public class OrderItem
    {
        public int OrderId { get; private set; }
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private OrderItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public OrderItem(string productId, string productName, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
        }
    }
}