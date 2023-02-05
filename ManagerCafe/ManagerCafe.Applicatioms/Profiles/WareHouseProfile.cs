using AutoMapper;
using ManagerCafe.Contracts.Dtos.WareHouseDtos;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Applications.Profiles
{
    public class WareHouseProfile : Profile
    {
        public WareHouseProfile()
        {
            CreateMap<WareHouse, WareHouseDto>();
            CreateMap<CreateWareHouseDto, WareHouse>();
            CreateMap<UpdateWareHouseDto, WareHouse>();
            CreateMap<FilterWareHouseDto, WareHouse>();
        }
    }
}
