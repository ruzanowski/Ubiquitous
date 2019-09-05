using System.Collections.Generic;
using MediatR;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.Application.Products.Queries.GetStatistics
{
    public class GetProductsStatisticsQuery :  IRequest<IList<ProductStatisticsDto>>
    {
        public ReportTimeStepFrequency StepFrequency { get; set; }
    }

    public enum ReportTimeStepFrequency
    {
        Secondly,
        Minutely,
        Hourly,
        Daily
    }
}