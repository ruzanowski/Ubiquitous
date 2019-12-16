using System.Collections.Generic;
using MediatR;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.Application.Products.Queries.GetStatisticsByCategory
{
    public class GetProductsStatisticsByCategory :  IRequest<IList<ProductByCategoryStatisticsDto>>
    {
    }
}