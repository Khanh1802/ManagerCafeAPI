using AutoMapper;
using ManagerCafe.Contracts.Dtos.Orders;
using ManagerCafe.Contracts.Dtos.ProductDtos;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Applications.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<FilterProductDto, Product>();

            CreateMap<Product, SearchProductDto>();
        }
    }
}
