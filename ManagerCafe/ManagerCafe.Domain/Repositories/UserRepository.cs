using ManagerCafe.Data.Data;
using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagerCafe.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ManagerCafeDbContext _contex;

        public UserRepository(ManagerCafeDbContext contex)
        {
            _contex = contex;
        }

        public async Task<User> AddAsync(User entity)
        {
            await _contex.Users.AddAsync(entity);
            entity.CreateTime = DateTime.Now;
            await _contex.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(User entity)
        {
            entity.IsDeleted = true;
            entity.DeletetionTime = DateTime.Now;
            await _contex.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _contex.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync<TKey>(TKey key)
        {
            return await _contex.Users.FindAsync(key);
        }

        public Task<IQueryable<User>> GetQueryableAsync()
        {
            return Task.FromResult(_contex.Users.AsQueryable());
        }

        public async Task<User> UpdateAsync(User entity)
        {
            entity.LastModificationTime = DateTime.Now;
            await _contex.SaveChangesAsync();
            return entity;
        }
    }
}
