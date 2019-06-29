using System;
using AutoMapper;
using U.SmartStoreAdapter.Api.Manufacturers;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

namespace U.SmartStoreAdapter.Application.MappingProfiles
{
    public class ManufacturerMappingProfile : Profile
    {
        public ManufacturerMappingProfile()
        {
            CreateMap<ManufacturerDto, Manufacturer>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest) => dest.Name ?? src.Name))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom((src, dest) => dest.Description ?? src.Description))
                .ForMember(dest => dest.Published, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOnUtc, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedOnUtc,
                    opt => opt.MapFrom((src, dest) =>
                        dest.CreatedOnUtc.Equals(DateTime.MinValue) ? DateTime.UtcNow : dest.CreatedOnUtc))
                .ForMember(dest => dest.PictureId, opt => opt.MapFrom(src => src.PictureId))
                .ForAllOtherMembers(x=>x.Ignore());

            CreateMap<Manufacturer, ManufacturerViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(dest => dest.Name))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom((src, dest) => dest.Description))
                .ForMember(dest => dest.PictureId, opt => opt.MapFrom(src => src.PictureId));
        }
    }
}