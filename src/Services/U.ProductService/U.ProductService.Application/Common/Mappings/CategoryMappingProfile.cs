using AutoMapper;
using U.ProductService.Application.Categories.Models;
using U.ProductService.Domain.Entities.Product;

namespace U.ProductService.Application.Common.Mappings
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryViewModel>();
        }
    }
}