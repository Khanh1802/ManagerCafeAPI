using ManagerCafe.Data.Data;
using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagerCafe.Domain.Repositories
{
    public class WareHouseRepository : IWareHouseRepository
    {
        private readonly ManagerCafeDbContext _context;

        public WareHouseRepository(ManagerCafeDbContext context)
        {
            _context = context;
        }

        public async Task<WareHouse> AddAsync(WareHouse entity)
        {
            await _context.AddAsync(entity);
            entity.CreateTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(WareHouse entity)
        {
            entity.IsDeleted = true;
            entity.DeletetionTime = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<List<WareHouse>> GetAllAsync()
        {
            return await _context.WareHouses.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<WareHouse> GetByIdAsync<TKey>(TKey key)
        {
            return await _context.WareHouses.FindAsync(key);
        }

        public async Task<IQueryable<WareHouse>> GetQueryableAsync()
        {
            return await Task.FromResult(_context.WareHouses.Where(x => !x.IsDeleted).AsQueryable());
        }

        public async Task<WareHouse> UpdateAsync(WareHouse entity)
        {
            entity.LastModificationTime = DateTime.Now;
            _context.WareHouses.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
