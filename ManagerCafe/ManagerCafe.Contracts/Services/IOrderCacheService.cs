using ManagerCafe.CacheItems.OrderDetails;

namespace ManagerCafe.Contracts.Services
{
    public interface IOrderCacheService
    {
        void SetOrder(OrderDetailCacheItem orderDetailCacheItem);
        OrderDetailCacheItem GetOrder();

    }
}
