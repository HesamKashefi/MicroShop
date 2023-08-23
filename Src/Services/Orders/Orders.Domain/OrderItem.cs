namespace Orders.Domain
{
    public class OrderItem
    {
        public int OrderId { get; private set; }
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }

        private OrderItem() { }

        public OrderItem(string productId, string productName, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
        }
    }
}