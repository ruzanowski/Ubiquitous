using System;
using AutoMapper;
using U.Common.Products;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

// ReSharper disable once SimplifyConditionalTernaryExpression
namespace U.SmartStoreAdapter.Application.Common.MappingProfiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<SmartProductDto, Product>()
                .ForMember(x => x.Id, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.UpdatedOnUtc, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedOnUtc,
                    opt => opt.MapFrom((src, dest) =>
                        dest.CreatedOnUtc.Equals(DateTime.MinValue) ? DateTime.UtcNow : dest.CreatedOnUtc));

            CreateMap<Product, SmartProductViewModel>();
        }
    }
}