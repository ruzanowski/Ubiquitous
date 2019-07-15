using System;
using AutoMapper;
using U.SmartStoreAdapter.Api.Products;
using U.SmartStoreAdapter.Domain.Entities.Catalog;

// ReSharper disable once SimplifyConditionalTernaryExpression
namespace U.SmartStoreAdapter.Application.MappingProfiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<SmartProductDto, Product>()
                .ForMember(x => x.Id, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.InStock))
                .ForMember(dest => dest.MinimumCustomerEnteredPrice,
                    opt => opt.MapFrom(src => src.PriceMinimumSpecifiedByCustomer))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(dest => dest.MetaTitle, opt => opt.MapFrom(x => x.Name))
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
                .ForMember(dest => dest.ShortDescription,
                    opt => opt.MapFrom((src, dest) =>
                        dest.ShortDescription ?? (src.Description.Length > 50
                            ? src.Description.Substring(0, 50)
                            : src.Description)))
                .ForMember(dest => dest.Sku,
                    opt => opt.MapFrom(src => $"{src.ManufacturerId}.{src.ProductUniqueCode}"))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceInTax))
                .ForMember(dest => dest.ProductCost, opt => opt.MapFrom(dto => dto.ProductCost))
                .ForMember(dest => dest.Published,
                    opt => opt.MapFrom((src, dest) =>
                        dest.Published && src.IsAvailable == false ? false : dest.Published))
                .ForMember(dest => dest.UpdatedOnUtc, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedOnUtc,
                    opt => opt.MapFrom((src, dest) =>
                        dest.CreatedOnUtc.Equals(DateTime.MinValue) ? DateTime.UtcNow : dest.CreatedOnUtc))
                .ForMember(dest => dest.MainPictureId, opt => opt.MapFrom(x => x.MainPictureId))
                .ForMember(x => x.ProductManufacturers, opt => opt.Ignore())
                .ForMember(x => x.ProductPictures, opt => opt.Ignore())
                .ForMember(dest => dest.MergedDataValues, opt => opt.Ignore())
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore())
                .ForMember(dest => dest.TaxCategoryId, opt => opt.MapFrom(src => src.TaxCategoryId))
                .ForMember(dest => dest.IsFreeShipping, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsShipEnabled, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.AllowCustomerReviews, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.AdminComment,
                    opt => opt.MapFrom((src, src2) =>
                        src2.AdminComment ?? $"Integrated by db.integrator at {DateTime.UtcNow} UTC time."))
                .ForSourceMember(x => x.ManufacturerId, opt => opt.DoNotValidate())
                .ForSourceMember(x => x.PicturesIds, opt => opt.DoNotValidate())
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
                .ForMember(dest => dest.PriceMinimumSpecifiedByCustomer,
                    opt => opt.MapFrom(src => src.MinimumCustomerEnteredPrice));
        }
    }
}