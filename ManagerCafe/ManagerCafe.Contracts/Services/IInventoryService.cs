using ManagerCafe.Contracts.Dtos.InventoryDtos;
using ManagerCafe.Share.Commons;

namespace ManagerCafe.Contracts.Services
{
    public interface IInventoryService : IGenericService<InventoryDto, CreatenInvetoryDto, UpdateInventoryDto, FilterInventoryDto, Guid>
    {
        Task<CommonPageDto<InventoryDto>> GetPagedListAsync(FilterInventoryDto item);
        Task<List<InventoryDto>> FindByIdProductAndWarehouse(FilterInventoryDto item);
        Task<InventoryOrderDetail> GetInventoryOrderDetail(Guid productId);

        //Task LogicProcessing(List<OrderDetailCacheItem> orderDetailCacheItems);
    }
}
