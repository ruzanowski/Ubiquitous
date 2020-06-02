using System.Collections.Generic;
using U.ProductService.Application.Pictures;
using U.ProductService.Application.Pictures.Models;

namespace U.ProductService.Application.Manufacturers.Models
{
    public class ManufacturerDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<PictureViewModel> Pictures { get; set; }
    }
}