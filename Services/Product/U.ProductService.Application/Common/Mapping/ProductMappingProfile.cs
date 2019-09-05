using AutoMapper;
using U.ProductService.Application.Events.IntegrationEvents.Events;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;

namespace U.ProductService.Application.Mapping
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

            CreateMap<ProductViewModel, ReportProductPayload>();
        }
    }
}