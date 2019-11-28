using AutoMapper;
using U.ProductService.Application.Categories.Models;
using U.ProductService.Domain.Aggregates.Category;

namespace U.ProductService.Application.Common.Mapping
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryViewModel>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryParentId, opt => opt.MapFrom(src => src.ParentCategoryId));
        }
    }
}