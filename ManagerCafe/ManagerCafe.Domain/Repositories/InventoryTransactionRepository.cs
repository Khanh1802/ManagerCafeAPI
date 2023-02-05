using ManagerCafe.Data.Data;
using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagerCafe.Domain.Repositories
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly ManagerCafeDbContext _context;

        public InventoryTransactionRepository(ManagerCafeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(InventoryTransaction item)
        {
            item.CreateTime = DateTime.Now;
            await _context.InventoryTransactions.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public Task<List<InventoryTransaction>> GetAllAsync()
        {
            return _context.InventoryTransactions.ToListAsync();
        }

        public async Task<InventoryTransaction> GetByIdAsync<T>(T key)
        {
            return await _context.InventoryTransactions.FindAsync(key);
        }

        public Task<IQueryable<InventoryTransaction>> GetQueryableAsync()
        {
            return Task.FromResult(_context.InventoryTransactions.AsQueryable());
        }
    }
}
