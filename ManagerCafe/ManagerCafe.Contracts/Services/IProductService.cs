using ManagerCafe.Contracts.Dtos.Orders;
using ManagerCafe.Contracts.Dtos.ProductDtos;
using ManagerCafe.Share.Commons;

namespace ManagerCafe.Contracts.Services
{
    public interface IProductService :
        IGenericService<ProductDto, CreateProductDto, UpdateProductDto, FilterProductDto, Guid>
    {
        Task<CommonPageDto<ProductDto>> GetPagedListAsync(FilterProductDto item);
        Task<CommonPageDto<SearchProductDto>> SearchProductAsync(FilterProductDto filter);
    }
}
