using AutoMapper;
using ManagerCafe.Contracts.Dtos.UserTypeDtos;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Applications.Profiles
{
    public class UserTypeProfile : Profile
    {
        public UserTypeProfile()
        {
            CreateMap<UserType, UserTypeDto>();
            CreateMap<CreateUserTypeDto, UserType>();
            CreateMap<FilterUserTypeDto, UserType>();
            CreateMap<UpdateUserTypeDto, UserType>();
        }
    }
}
