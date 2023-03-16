using AutoMapper;
using ManagerCafe.Contracts.Dtos.CartDtos;

namespace ManagerCafe.Applications.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CreateCartDto, CartDto>();
        }
    }
}
