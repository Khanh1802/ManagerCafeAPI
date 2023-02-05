namespace ManagerCafe.Domain.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> UpdateAsync(TEntity entity);
        Task Delete(TEntity entity);
        Task<TEntity> GetByIdAsync<TKey>(TKey key);
        Task<IQueryable<TEntity>> GetQueryableAsync();
    }
}
