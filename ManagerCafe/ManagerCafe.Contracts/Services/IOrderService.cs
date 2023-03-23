using ManagerCafe.Contracts.Dtos.OrderDetails;
using ManagerCafe.Contracts.Dtos.Orders;
using ManagerCafe.Share.Commons;

namespace ManagerCafe.Contracts.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(CreateOrderDto item);
        Task<CommonPageDto<OrderDto>> GetAsync(FilterOrderDto item);
        Task<List<OrderDetailDto>> GetById(Guid id);
    }
}
