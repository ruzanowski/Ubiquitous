using AutoMapper;
using U.SmartStoreAdapter.Application.Models.Categories;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.MappingProfiles
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryViewModel>()
                .ForMember(dest=>dest.Id,opt=>opt.MapFrom(src=>src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src=>src.Name))
                .ForMember(dest=>dest.Description,opt=>opt.MapFrom(src=>src.Description))
                .ForMember(dest=>dest.PictureId,opt=>opt.MapFrom(src=>src.PictureId))
                .ForMember(dest=>dest.ParentCategoryId,opt=>opt.MapFrom(src=>src.ParentCategoryId))
                .ForAllOtherMembers(x=>x.Ignore());
        }
    }
}