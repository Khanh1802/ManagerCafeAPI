using AutoMapper;
using ManagerCafe.CacheItems.OrderDetails;
using ManagerCafe.Contracts.Dtos.OrderDetails;
namespace ManagerCafe.Applications.Profiles
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetailDto, OrderDetailCacheItem>();
            CreateMap<OrderDetailCacheItem, OrderDetailDto>();
        }
    }
}
