using ManagerCafe.Contracts.Dtos.Orders;

namespace ManagerCafe.Contracts.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(CreateOrderDto item);
        Task DeleteAsync(Guid id);
    }
}
