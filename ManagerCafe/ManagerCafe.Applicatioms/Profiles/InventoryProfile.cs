using AutoMapper;
using ManagerCafe.Contracts.Dtos.InventoryDtos;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Applications.Profiles
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<Inventory, InventoryDto>();
            //.ForMember(x => x.ProductName, opt => opt.MapFrom(o => o.Product != null ? o.Product.Name : string.Empty));
            CreateMap<CreatenInvetoryDto, Inventory>();
            CreateMap<FilterInventoryDto, Inventory>();
            CreateMap<UpdateInventoryDto, Inventory>();
            CreateMap<InventoryDto, UpdateInventoryDto>();
        }
    }
}
