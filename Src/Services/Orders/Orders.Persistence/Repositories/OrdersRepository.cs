using Common.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _db.Database.BeginTransactionAsync();
        }

        public async Task<PagedResult<OrderDto[]>> GetAllOrdersAsync(int page = 1)
        {
            var query = _db.Orders
                .Skip((page - 1) * Pager.DefaultPageSize)
                .Take(Pager.DefaultPageSize)
                .Select(order => new OrderDto(order.Id, order.CreatedAt, order.Address, order.Status))
                .AsNoTracking();
            var data = await query.ToArrayAsync();
            var count = await query.CountAsync();

            return Result.Success(data, new(count, page));
        }

        public async Task<PagedResult<OrderDto[]>> GetBuyerOrdersAsync(int buyerId, int page = 1)
        {
            var query = _db.Orders
                .Where(order => order.BuyerId == buyerId)
                .Skip((page - 1) * Pager.DefaultPageSize)
                .Take(Pager.DefaultPageSize)
                .Select(order => new OrderDto(order.Id, order.CreatedAt, order.Address, order.Status))
                .AsNoTracking();
            var data = await query.ToArrayAsync();
            var count = await query.CountAsync();

            return Result.Success(data, new(count, page));
        }

        public async Task<Order?> GetOrderAsync(int orderId)
        {
            return await _db.Orders
                .Include(order => order.OrderItems)
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
