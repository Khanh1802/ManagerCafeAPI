using ManagerCafe.Commons;

namespace ManagerCafe.Contracts.Services
{
    public interface IGenericService<TEntityDto, in TCreateaDto, in UpdateDto, in FilterDto, in TKey>
        where TEntityDto : class
        where TCreateaDto : class
        where UpdateDto : class
        where FilterDto : class
    {
        Task<TEntityDto> AddAsync(TCreateaDto item);
        Task<List<TEntityDto>> GetAllAsync();
        Task<TEntityDto> UpdateAsync(UpdateDto item);
        Task DeleteAsync(TKey key);
        Task<TEntityDto> GetByIdAsync(TKey key);
        Task<List<TEntityDto>> FilterAsync(FilterDto item);
    }
}
