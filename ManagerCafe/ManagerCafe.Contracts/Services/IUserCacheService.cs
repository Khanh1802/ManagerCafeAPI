using ManagerCafe.Share.CacheItems.Users;

namespace ManagerCafe.Contracts.Services
{
    public interface IUserCacheService
    {
        UserCacheItem GetOrDefault();

        void Set(UserCacheItem userCacheItem);
    }
}
