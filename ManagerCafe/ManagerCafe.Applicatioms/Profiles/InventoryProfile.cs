using AutoMapper;
using ManagerCafe.Contracts.Dtos.InventoryDtos;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Applications.Profiles
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<Inventory, InventoryDto>()
                .ForMember(x => x.Price,o => o.MapFrom(k => k.Product.PriceSell));
            CreateMap<Inventory, ProductInventoryDto>()
                .ForMember(x => x.Quantity, o => o.MapFrom(x => x.Quatity))
                .ForMember(x => x.WarehouseName, o => o.MapFrom(x => x.WareHouse.Name));
            CreateMap<CreatenInvetoryDto, Inventory>();
            CreateMap<FilterInventoryDto, Inventory>();
            CreateMap<UpdateInventoryDto, Inventory>();
            CreateMap<InventoryDto, UpdateInventoryDto>();
        }
    }
}
