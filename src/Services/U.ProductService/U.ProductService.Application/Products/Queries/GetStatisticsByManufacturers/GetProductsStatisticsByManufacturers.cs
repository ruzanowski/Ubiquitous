using System.Collections.Generic;
using MediatR;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.Application.Products.Queries.GetStatisticsByManufacturers
{
    public class GetProductsStatisticsByManufacturers :  IRequest<IList<ProductByManufacturersStatisticsDto>>
    {
    }
}