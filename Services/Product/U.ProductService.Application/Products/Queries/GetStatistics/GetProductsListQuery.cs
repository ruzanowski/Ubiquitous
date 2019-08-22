using System.Collections.Generic;
using MediatR;

namespace U.ProductService.Application.Products.Queries.QueryProducts
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