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
            CreateMap<CartDetailDto, OrderDetail>();
        }
    }
}
