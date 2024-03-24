using AutoMapper;
using RetailPrice.DTO;
using RetailPrice.Models;

namespace RetailPrice.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            
            CreateMap<Store, StoreDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();
            CreateMap<Pricing, PricingDto>().ReverseMap();

            // ...additional mappings...
        }
    }
}
