using AutoMapper;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Domain.Entities.Manufacturer;
using U.ProductService.Domain.Entities.Picture;
using U.ProductService.Domain.Entities.Product;

namespace U.ProductService.Application.Common.Mappings
{
    public class PictureMappingProfile : Profile
    {
        public PictureMappingProfile()
        {
            CreateMap<Picture, PictureViewModel>();
            CreateMap<ProductPicture, PictureViewModel>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Picture.Id))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Picture.Description))
                .ForMember(dest => dest.Url,
                    opt => opt.MapFrom(src => src.Picture.Url))
                .ForMember(dest => dest.FileName,
                    opt => opt.MapFrom(src => src.Picture.FileName))
                .ForMember(dest => dest.MimeTypeId,
                    opt => opt.MapFrom(src => src.Picture.MimeTypeId))
                .ForMember(dest => dest.PictureAddedAt,
                    opt => opt.MapFrom(src => src.Picture.PictureAddedAt))
                .ForMember(dest => dest.FileStorageUploadId,
                opt => opt.MapFrom(src => src.Picture.FileStorageUploadId));

            CreateMap<ManufacturerPicture, PictureViewModel>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Picture.Id))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Picture.Description))
                .ForMember(dest => dest.Url,
                    opt => opt.MapFrom(src => src.Picture.Url))
                .ForMember(dest => dest.FileName,
                    opt => opt.MapFrom(src => src.Picture.FileName))
                .ForMember(dest => dest.MimeTypeId,
                    opt => opt.MapFrom(src => src.Picture.MimeTypeId))
                .ForMember(dest => dest.PictureAddedAt,
                    opt => opt.MapFrom(src => src.Picture.PictureAddedAt))
                .ForMember(dest => dest.FileStorageUploadId,
                    opt => opt.MapFrom(src => src.Picture.FileStorageUploadId));
        }
    }
}