using AutoMapper;
using ManagerCafe.Contracts.Dtos.OrderDetails;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Applications.Profiles
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, OrderDetailDto>();
            //CreateMap<Create>
        }
    }
}
