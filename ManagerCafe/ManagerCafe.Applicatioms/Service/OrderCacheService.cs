using ManagerCafe.CacheItems.OrderDetails;
using ManagerCafe.Contracts.Services;
using Microsoft.Extensions.Caching.Memory;

namespace ManagerCafe.Applications.Service
{
    public class OrderCacheService : IOrderCacheService
    {
        private const string CacheOrder = "key_Order";
        private readonly IMemoryCache _memoryCache;

        public OrderCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public OrderDetailCacheItem GetOrder()
        {
            return _memoryCache.Get<OrderDetailCacheItem>(CacheOrder);
        }

        public void SetOrder(OrderDetailCacheItem orderDetailCacheItem)
        {
            _memoryCache.Set(CacheOrder, orderDetailCacheItem);
        }
    }
}
