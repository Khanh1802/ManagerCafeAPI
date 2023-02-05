using ManagerCafe.Contracts.Dtos.Orders;

namespace ManagerCafe.Contracts.Services
{
    public interface IOrderService : IGenericService<OrderDto, CreateOrderDto, UpdateOrderDto, FilterOrderDto, Guid>
    {
        void SetCacheOrder();

    }
}
