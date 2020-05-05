using System;
using System.Linq;
using AutoMapper;
using U.SmartStoreAdapter.Application.Products;
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
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.InStock))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(dest => dest.Length,
                    opt => opt.MapFrom((src, src2) => src.Length == 0 ? src2.Length : src.Length))
                .ForMember(dest => dest.Width,
                    opt => opt.MapFrom((src, src2) => src.Width == 0 ? src2.Width : src.Width))
                .ForMember(dest => dest.Height,
                    opt => opt.MapFrom((src, src2) => src.Height == 0 ? src2.Height : src.Height))
                .ForMember(dest => dest.Weight,
                    opt => opt.MapFrom((src, src2) => src.Weight == 0 ? src2.Weight : src.Weight))
                .ForMember(dest => dest.FullDescription, opt =>
                    opt.MapFrom((src, dest) => dest.FullDescription ?? src.Description))
                .ForMember(dest => dest.Sku,
                    opt => opt.MapFrom(src => $"{src.ManufacturerId}.{src.ProductUniqueCode}"))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceInTax))
                .ForMember(dest => dest.Published,
                    opt => opt.MapFrom((src, dest) =>
                        dest.Published && src.IsAvailable == false ? false : dest.Published))
                .ForMember(dest => dest.UpdatedOnUtc, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedOnUtc,
                    opt => opt.MapFrom((src, dest) =>
                        dest.CreatedOnUtc.Equals(DateTime.MinValue) ? DateTime.UtcNow : dest.CreatedOnUtc))
                .ForMember(x => x.ProductManufacturers, opt => opt.Ignore())
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore())
                .ForMember(dest => dest.AdminComment,
                    opt => opt.MapFrom((src, src2) =>
                        src2.AdminComment ?? $"Updated externally at {DateTime.UtcNow} UTC time."))
                .ForSourceMember(x => x.ManufacturerId, opt => opt.DoNotValidate())
                .ForAllOtherMembers(x => x.Ignore());


            CreateMap<Product, SmartProductViewModel>()
                .ForMember(dest => dest.ProductUniqueCode,
                    opt => opt.MapFrom(src => src.SystemName))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.FullDescription))
                .ForMember(dest => dest.InStock,
                    opt => opt.MapFrom(src => src.StockQuantity))
                .ForMember(dest => dest.IsPublished,
                    opt => opt.MapFrom(src => src.Published))
                .ForMember(dest => dest.PriceInTax,
                    opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CategoryId,
                    opt => opt.MapFrom(src =>
                        src.ProductCategories.FirstOrDefault().CategoryId))
                .ForMember(dest => dest.ManufacturerId,
                    opt => opt.MapFrom(src =>
                        src.ProductManufacturers.FirstOrDefault().ManufacturerId));;
        }
    }
}