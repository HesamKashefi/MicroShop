namespace Orders.Domain
{
    public class Order : Entity<int>
    {
        public int BuyerId { get; init; }

        public DateTime CreatedAt { get; private init; } = DateTime.Now;
        public required Address Address { get; init; }

        public OrderStatusEnum Status { get; private set; }
        public bool IsPaid { get; private set; }

        private List<OrderItem> _orderItems = new();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public void Add(OrderItem orderItem)
        {
            ArgumentNullException.ThrowIfNull(orderItem);
            _orderItems.Add(orderItem);
        }

        public void MarkAsPaid()
        {
            // payment can be done either online or in person
            if (this.Status is OrderStatusEnum.Submitted or OrderStatusEnum.Shipped)
            {
                this.IsPaid = true;
            }
        }

        public void SetStatusApproved()
        {
            if (this.Status == OrderStatusEnum.Submitted)
            {
                this.Status = OrderStatusEnum.Approved;
            }
        }

        public void SetStatusShipped()
        {
            if (this.Status == OrderStatusEnum.Approved)
            {
                this.Status = OrderStatusEnum.Shipped;
            }
        }

        public void SetStatusCaneclled()
        {
            if (this.Status != OrderStatusEnum.Shipped)
            {
                this.Status = OrderStatusEnum.Cancelled;
            }
        }
    }
}