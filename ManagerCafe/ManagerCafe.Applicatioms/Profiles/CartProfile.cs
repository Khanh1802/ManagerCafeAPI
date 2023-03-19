using AutoMapper;
using ManagerCafe.Contracts.Dtos.CartDtos;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Applications.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CreateCartDto, CartDetailDto>();
            CreateMap<CartDetailDto, OrderDetail>()
                .ForMember(x => x.CreateTime, o => o.MapFrom(k => DateTime.Now));
        }
    }
}
