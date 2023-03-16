using ManagerCafe.Contracts.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace ManagerCafe.Applications.Service
{
    public class OrderDetailCacheService : IOrderDetailCacheService
    {
        private const string CacheOrderDetail = "key_OrderDetail";
        private readonly IMemoryCache _memoryCache;

        public OrderDetailCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        //public List<OrderDetailCacheItem> GetOrderDetails()
        //{
        //    return _memoryCache.Get<List<OrderDetailCacheItem>>(CacheOrderDetail);
        //}

        //public void SetOrderDetails()
        //{
        //    _memoryCache.Set(CacheOrderDetail, new List<OrderDetailCacheItem>());
        //}
    }
}
