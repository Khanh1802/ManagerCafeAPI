using ManagerCafe.Data.Data;
using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagerCafe.Domain.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ManagerCafeDbContext _context;

        public InventoryRepository(ManagerCafeDbContext context)
        {
            _context = context;
        }

        public async Task<Inventory> AddAsync(Inventory entity)
        {
            await _context.Invetories.AddAsync(entity);
            entity.CreateTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Inventory>> DataGirdView(Product product, WareHouse warehouse)
        {
            return await _context.Invetories
                .Include(x => x.WareHouse.Id == warehouse.Id)
                .Include(x => x.Product.Id == product.Id)
                .ToListAsync();
        }

        public async Task Delete(Inventory entity)
        {
            entity.IsDeleted = true;
            entity.DeletetionTime = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Inventory>> GetAllAsync()
        {
            return await _context.Invetories.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<Inventory> GetByIdAsync<TKey>(TKey key)
        {
            var entity = await _context.Invetories.FindAsync(key);
            return entity;
        }

        public async Task<IQueryable<Inventory>> GetQueryableAsync()
        {
            return await Task.FromResult(_context.Invetories.AsQueryable().Where(x => !x.IsDeleted));
        }

        public async Task<Inventory> UpdateAsync(Inventory entity)
        {
            _context.Invetories.Update(entity);
            entity.LastModificationTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return entity;
        }

    }
}
