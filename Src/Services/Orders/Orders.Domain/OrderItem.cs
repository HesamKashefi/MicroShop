namespace Orders.Domain
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public string ProductId { get; }
        public string ProductName { get; }
        public int Quantity { get; }

        public OrderItem(string productId, string productName, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
        }
    }
}