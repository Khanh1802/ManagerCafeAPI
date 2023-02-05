using ManagerCafe.CacheItems.OrderDetails;
using ManagerCafe.Commons;
using ManagerCafe.Contracts.Dtos.InventoryDtos;

namespace ManagerCafe.Contracts.Services
{
    public interface IInventoryService : IGenericService<InventoryDto, CreatenInvetoryDto, UpdateInventoryDto, FilterInventoryDto, Guid>
    {
        Task<CommonPageDto<InventoryDto>> GetPagedListAsync(FilterInventoryDto item, int choice);
        Task<List<InventoryDto>> FindByIdProductAndWarehouse(FilterInventoryDto item);
        Task<InventoryOrderDetail> GetInventoryOrderDetail(Guid productId);

        Task LogicProcessing(List<OrderDetailCacheItem> orderDetailCacheItems);
    }
}
