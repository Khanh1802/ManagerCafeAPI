using AutoMapper;
using ManagerCafe.Contracts.Dtos.OrderDetails;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Applications.Profiles
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<CreateOrderDetailDto, OrderDetail>();
            CreateMap<OrderDetail, OrderDetailDto>();
        }
    }
}
