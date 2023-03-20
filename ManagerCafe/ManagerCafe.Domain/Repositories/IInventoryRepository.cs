using ManagerCafe.Data.Models;

namespace ManagerCafe.Domain.Repositories
{
    public interface IInventoryRepository : IGenericRepository<Inventory>
    {
        public Task<List<Inventory>> UpdateAsync(List<Inventory> entities);
    }
}
