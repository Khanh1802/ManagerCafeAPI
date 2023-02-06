using ManagerCafe.Commons;

namespace ManagerCafe.Contracts.Services
{
    public interface IGenericService<TEntityDto, in TCreateDto, in UpdateDto, in FilterDto, in TKey>
        where TEntityDto : class
        where TCreateDto : class
        where UpdateDto : class
        where FilterDto : class
    {
        Task<TEntityDto> AddAsync(TCreateDto item);
        Task<List<TEntityDto>> GetAllAsync();
        Task<TEntityDto> UpdateAsync(TKey id, UpdateDto item);
        Task DeleteAsync(TKey key);
        Task<TEntityDto> GetByIdAsync(TKey key);
        Task<List<TEntityDto>> FilterAsync(FilterDto item);
    }
}