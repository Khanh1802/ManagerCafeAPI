using ManagerCafe.Data.Data;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Domain.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ManagerCafeDbContext _context;

        public OrderRepository(ManagerCafeDbContext context)
        {
            _context = context;
        }

        public async Task<Order> AddAsync(Order entity)
        {
            entity.CreateTime = DateTime.Now;
            await _context.Orders.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Order entity)
        {
            _context.Orders.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Task<List<Order>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetByIdAsync<TKey>(TKey key)
        {
            return await _context.Orders.FindAsync(key);
        }

        public Task<IQueryable<Order>> GetQueryableAsync()
        {
            return Task.FromResult(_context.Orders.AsQueryable());
        }

        public async Task<Order> UpdateAsync(Order entity)
        {
            entity.LastModificationTime = DateTime.Now;
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
