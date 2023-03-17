using AutoMapper;
using ManagerCafe.Contracts.Dtos.Orders;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Applications.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<CreateOrderDto,Order>();
        }
    }
}
