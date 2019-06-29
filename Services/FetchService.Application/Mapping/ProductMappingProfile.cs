using System;
using System.Linq;
using AutoMapper;
using U.FetchService.Application.Models;
using U.FetchService.Domain.Entities.Common;
using U.FetchService.Domain.Entities.Picture;
using U.FetchService.Domain.Entities.Product;
using U.FetchService.Domain.Entities.ProductTags;
using U.SmartStoreAdapter.Api.Products;

namespace U.FetchService.Application.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductMappingProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public ProductMappingProfile()
        {
            CreateMap<SmartProductViewModel, Product>()
                .ForMember(x => x.Created, opt => opt.MapFrom(src => new Modified
                {
                    UserId = Guid.NewGuid(),
                    ModifiedAt = DateTime.Now
                }))
                .ForMember(x => x.LastModified, opt => opt.MapFrom(src => new Modified
                {
                    UserId = Guid.NewGuid(),
                    ModifiedAt = DateTime.Now
                }))
                .ForMember(x => x.ProductTags, opt => opt.Ignore())
                .ForMember(x => x.Pictures, opt => opt.Ignore())
                .AfterMap((model, product) =>
                {
                    if (model.PicturesIds != null)
                        product.SetPictures(model.PicturesIds?.Select(x => new Picture
                        {
                            Id = x,
                            Comment = "RandomComment"
                        }));

                    if (model.ProductTags != null)
                        product.SetProductTags(model.ProductTags.Select(x => new ProductTag
                        {
                            Id = x,
                            Tag = "RandomTag"
                        }));
                });
        }
    }
}