using ManagerCafe.Data.Data;
using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagerCafe.Domain.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ManagerCafeDbContext _context;

        public ProductRepository(ManagerCafeDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _context.Products.AddAsync(entity);
            entity.CreateTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Product entity)
        {
            entity.IsDeleted = true;
            entity.DeletetionTime = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<Product> GetByIdAsync<TKey>(TKey key)
        {
            return await _context.Products.FindAsync(key);
        }

        public async Task<IQueryable<Product>> GetQueryableAsync()
        {
            return await Task.FromResult(_context.Products.Where(x => !x.IsDeleted).AsQueryable());
        }

        public async Task<Product> UpdateAsync(Product entity)
        {
            _context.Products.Update(entity);
            entity.LastModificationTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
