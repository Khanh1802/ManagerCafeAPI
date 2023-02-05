using ManagerCafe.CacheItems.Users;
using ManagerCafe.Contracts.Services;
using Microsoft.Extensions.Caching.Memory;

namespace ManagerCafe.Applications.Service
{
    public class UserCacheService : IUserCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private const string LoginKey = "LoginKey";

        public UserCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public UserCacheItem GetOrDefault()
        {
            return _memoryCache.Get<UserCacheItem>(LoginKey);
        }

        public void Set(UserCacheItem userCacheItem)
        {
            _memoryCache.Set(LoginKey, userCacheItem);
        }
    }
}
