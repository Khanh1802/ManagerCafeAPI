using AutoMapper;
using ManagerCafe.CacheItems.Users;
using ManagerCafe.Contracts.Dtos.UsersDtos;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Applications.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<FilterUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            CreateMap<User, UserCacheItem>();
        }
    }
}
