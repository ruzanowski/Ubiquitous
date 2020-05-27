using AutoMapper;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Application.Products.Commands.Update;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Product;

namespace U.ProductService.Application.Common.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(x => x.CreatedDateTime, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(x => x.LastFullUpdateDateTime, opt => opt.MapFrom(src => src.LastUpdatedAt));

            CreateMap<Picture, PictureViewModel>()
                .ForMember(x => x.MimeType, opt => opt.MapFrom(src => src.MimeType.Name));

            CreateMap<UpdateProductCommand, Product>()
                .ReverseMap()
                .ForMember(x => x.Dimensions, opt => opt.MapFrom(y => y.Dimensions))
                .ForMember(x => x.ProductId, opt => opt.MapFrom(y => y.Id))
                .ForAllOtherMembers(z => z.Ignore());


        }
    }
}