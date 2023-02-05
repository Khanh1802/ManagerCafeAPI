using ManagerCafe.Data.Data;
using ManagerCafe.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagerCafe.Domain.Repositories
{
    public class UserTypeRepository : IUserTypeRepository
    {
        private readonly ManagerCafeDbContext _contex;

        public UserTypeRepository(ManagerCafeDbContext contex)
        {
            _contex = contex;
        }

        public async Task<UserType> AddAsync(UserType entity)
        {
            entity.CreateTime = DateTime.Now;
            await _contex.AddAsync(entity);
            await _contex.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(UserType entity)
        {
            entity.IsDeleted = true;
            entity.DeletetionTime = DateTime.Now;
            await _contex.SaveChangesAsync();
        }

        public async Task<List<UserType>> GetAllAsync()
        {
            return await _contex.UserTypes.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<UserType> GetByIdAsync<TKey>(TKey key)
        {
            return await _contex.UserTypes.FindAsync(key);
        }

        public Task<IQueryable<UserType>> GetQueryableAsync()
        {
            return Task.FromResult(_contex.UserTypes.AsQueryable());
        }

        public async Task<UserType> UpdateAsync(UserType entity)
        {
            entity.LastModificationTime = DateTime.Now;
            await _contex.SaveChangesAsync();
            return entity;
        }
    }
}
