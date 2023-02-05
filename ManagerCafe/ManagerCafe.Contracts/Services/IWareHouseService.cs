using ManagerCafe.Commons;
using ManagerCafe.Contracts.Dtos.WareHouseDtos;

namespace ManagerCafe.Contracts.Services
{
    public interface IWareHouseService : IGenericService<WareHouseDto, CreateWareHouseDto, UpdateWareHouseDto, FilterWareHouseDto, Guid>
    {
        Task<List<WareHouseDto>> FilterChoice(int filter);
        Task<CommonPageDto<WareHouseDto>> GetPagedListAsync(FilterWareHouseDto item);
    }
}
