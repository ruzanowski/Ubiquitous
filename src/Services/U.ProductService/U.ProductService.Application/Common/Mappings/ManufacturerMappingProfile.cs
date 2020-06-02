using AutoMapper;
using U.ProductService.Application.Manufacturers.Models;
using U.ProductService.Domain.Entities.Manufacturer;

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