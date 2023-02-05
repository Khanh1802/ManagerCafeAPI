using ManagerCafe.Contracts.Dtos.OrderDetails;

namespace ManagerCafe.Contracts.Services
{
    public interface IOrderDetailService
    {
        Task AddAsync(OrderDetailDto item);
        void Delete(OrderDetailDto item);
    }
}
