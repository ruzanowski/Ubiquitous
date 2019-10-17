using AutoMapper;
using U.ProductService.Application.Events.IntegrationEvents.Events;
using U.ProductService.Application.Manufacturers.Models;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Manufacturer;

namespace U.ProductService.Application.Common.Mapping
{
    public class ManufacturerMappingProfile : Profile
    {
        public ManufacturerMappingProfile()
        {
            CreateMap<Manufacturer, ManufacturerViewModel>();
        }
    }
}