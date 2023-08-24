using Common.Data;
using Microsoft.EntityFrameworkCore;
using Orders.Domain;
using Orders.Domain.Contracts;
using Orders.Domain.Dtos;

namespace Orders.Persistence.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrdersDb _db;

        public OrdersRepository(OrdersDb db)
        {
            _db = db;
        }

        public async Task<PagedResult<OrderDto[]>> GetBuyerOrdersAsync(int buyerId, int page = 1)
        {
            var query = _db.Orders
                .Where(order => order.BuyerId == buyerId)
                .Skip((page - 1) * PagedResult.PageSize)
                .Take(PagedResult.PageSize)
                .Select(order => new OrderDto(order.Id, order.CreatedAt, order.Address, order.Status))
                .AsNoTracking();
            var data = await query.ToArrayAsync();
            var count = await query.CountAsync();
            return new PagedResult<OrderDto[]>
            {
                Data = data,
                CurrentPage = page,
                TotalCount = count,
                TotalPages = (int)Math.Ceiling((double)count / PagedResult.PageSize)
            };
        }

        public async Task<Order?> GetOrderAsync(int orderId)
        {
            return await _db.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(order => order.BuyerId == orderId);
        }

        public async Task SaveOrderAsync(Order order)
        {
            if (order.Id == 0)
            {
                _db.Orders.Add(order);
            }
            else
            {
                _db.Entry(order).State = EntityState.Modified;
            }
            await _db.SaveChangesAsync();
        }
    }
}
