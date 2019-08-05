using AutoMapper;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain.Aggregates;

namespace U.ProductService.Application.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.Pictures, opt => opt.Ignore());
        }
    }
}