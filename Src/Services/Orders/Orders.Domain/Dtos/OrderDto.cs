﻿namespace Orders.Domain.Dtos
{
    public record OrderDto(int Id, DateTime CreatedAt, Address Address, OrderStatusEnum Status, bool IsPaid);
}
