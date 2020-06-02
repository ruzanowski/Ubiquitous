using AutoMapper;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;
using U.ProductService.Domain.Entities.Product;

namespace U.ProductService.Application.Common.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<Dimensions, DimensionsDto>();

        }
    }
}