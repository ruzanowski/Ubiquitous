using AutoMapper;
using U.ProductService.Application.Manufacturers.Models;
using U.ProductService.Application.Pictures;
using U.ProductService.Domain.Entities.Manufacturer;
using U.ProductService.Domain.Entities.Product;

namespace U.ProductService.Application.Common.Mappings
{
    public class ManufacturerMappingProfile : Profile
    {
        public ManufacturerMappingProfile()
        {
            CreateMap<Manufacturer, ManufacturerViewModel>();
        }
    }
}