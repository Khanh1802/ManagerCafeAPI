using ManagerCafe.Commons;
using ManagerCafe.Contracts.Dtos.InventoryTransactionDtos;

namespace ManagerCafe.Contracts.Services
{
    public interface IInventoryTransactionService
    {
        Task<List<InventoryTransactionDto>> GetAllAsync();
        Task AddAsync(CreateInventoryTransactionDto item);
        Task<List<InventoryTransactionDto>> FilterStatisticFindAsync(FilterInventoryTransactionDto item);
        Task<List<InventoryTransactionDto>> FilterHistoryFindAsync(FilterInventoryTransactionDto item);
        Task<CommonPageDto<InventoryTransactionDto>> GetPagedStatisticListAsync(FilterInventoryTransactionDto item);
        Task<CommonPageDto<InventoryTransactionDto>> GetPagedHistoryListAsync(FilterInventoryTransactionDto item);
    }
}
