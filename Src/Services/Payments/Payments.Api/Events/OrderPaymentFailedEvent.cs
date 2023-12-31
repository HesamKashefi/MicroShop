﻿using EventBus.Core;

namespace Payments.Api.Events
{
    public class OrderPaymentFailedEvent : Event
    {
        public OrderPaymentFailedEvent(int buyerId, string buyerName, int orderId)
        {
            BuyerId = buyerId;
            BuyerName = buyerName;
            OrderId = orderId;
        }

        public int BuyerId { get; }
        public string BuyerName { get; }
        public int OrderId { get; }
    }
}
