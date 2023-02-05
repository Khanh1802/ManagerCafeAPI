using ManagerCafe.Commons;
using ManagerCafe.Contracts.Dtos.Orders;
using ManagerCafe.Contracts.Dtos.ProductDtos;

namespace ManagerCafe.Contracts.Services
{
    public interface IProductService :
        IGenericService<ProductDto, CreateProductDto, UpdateProductDto, FilterProductDto, Guid>
    {
        Task<CommonPageDto<ProductDto>> GetPagedListAsync(FilterProductDto item, int choice);
        Task<CommonPageDto<SearchProductDto>> SearchProductAsync(FilterProductDto filter);
    }
}
