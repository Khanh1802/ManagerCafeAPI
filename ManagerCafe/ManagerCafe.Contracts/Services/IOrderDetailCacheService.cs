using ManagerCafe.CacheItems.OrderDetails;

namespace ManagerCafe.Contracts.Services
{
    public interface IOrderDetailCacheService
    {
        void SetOrderDetails();
        List<OrderDetailCacheItem> GetOrderDetails();
    }
}
