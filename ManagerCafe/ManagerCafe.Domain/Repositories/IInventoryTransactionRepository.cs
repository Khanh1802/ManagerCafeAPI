using ManagerCafe.Data.Models;

namespace ManagerCafe.Domain.Repositories
{
    public interface IInventoryTransactionRepository
    {
        Task<List<InventoryTransaction>> GetAllAsync();
        Task AddAsync(InventoryTransaction item);
        Task<InventoryTransaction> GetByIdAsync<T>(T key);
        Task<IQueryable<InventoryTransaction>> GetQueryableAsync();
        Task AddAsync(List<InventoryTransaction> items);
    }
}
