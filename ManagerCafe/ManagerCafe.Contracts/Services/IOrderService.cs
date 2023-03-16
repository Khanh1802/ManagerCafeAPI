using ManagerCafe.Contracts.Dtos.Orders;
using ManagerCafe.Contracts.Dtos.ProductDtos;

namespace ManagerCafe.Contracts.Services
{
    public interface IOrderService /*: IGenericService<OrderDto, CreateOrderDto, UpdateProductDto, FilterOrderDto, Guid>*/
    {
        void SetCacheOrder();

    }
}
