using System.Collections.Generic;
using MediatR;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.Application.Products.Queries.GetStatistics
{
    public class GetProductsStatisticsQuery :  IRequest<IList<ProductStatisticsDto>>
    {
        public GroupByTimeStepFrequency StepFrequency { get; set; }
    }

    public enum GroupByTimeStepFrequency
    {
        Hourly,
        Daily
    }
}