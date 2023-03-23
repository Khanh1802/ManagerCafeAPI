using ManagerCafe.Contracts.Dtos.InventoryDtos;
using ManagerCafe.Share.Commons;

namespace ManagerCafe.Contracts.Services
{
    public interface IInventoryService : IGenericService<InventoryDto, CreatenInvetoryDto, UpdateInventoryDto, FilterInventoryDto, Guid>
    {
        Task<CommonPageDto<InventoryDto>> GetPagedListAsync(FilterInventoryDto item);
        Task<CommonPageDto<InventoryDto>> FindByIdProductAndWarehouse(FilterInventoryDto item);
        Task<List<ProductInventoryDto>> GetProductInventoryAsync(Guid productId);
        Task<Dictionary<Guid, List<ProductInventoryDto>>> GetProductInventoryAsync(List<Guid> productIds);
        Task<CommonPageDto<InventoryDto>> GetProductAndQuantityInventory(FilterInventoryDto item);
        //Task LogicProcessing(List<OrderDetailCacheItem> orderDetailCacheItems);
    }
}
