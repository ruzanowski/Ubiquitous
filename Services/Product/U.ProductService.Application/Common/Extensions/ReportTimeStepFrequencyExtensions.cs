using System;
using U.ProductService.Application.Products.Queries.QueryProducts;
using U.ProductService.Application.Products.Queries.QueryStatistics;

namespace U.ProductService.Application.Common.Extensions
{
    public static class ReportTimeStepFrequencyExtensions
    {
        public static DateTimePropertiesInclude InitializeDtIncludes(this ReportTimeStepFrequency step)
        {
            DateTimePropertiesInclude dateInclude = new DateTimePropertiesInclude();
            switch (step)
            {
                case ReportTimeStepFrequency.Secondly:
                    dateInclude.Year = true;
                    dateInclude.Month = true;
                    dateInclude.Day = true;
                    dateInclude.Hour = true;
                    dateInclude.Minute = true;
                    dateInclude.Second = true;
                    break;
                case ReportTimeStepFrequency.Minutely:
                    dateInclude.Year = true;
                    dateInclude.Month = true;
                    dateInclude.Day = true;
                    dateInclude.Hour = true;
                    dateInclude.Minute = true;
                    dateInclude.Second = false;
                    break;
                case ReportTimeStepFrequency.Hourly:
                    dateInclude.Year = true;
                    dateInclude.Month = true;
                    dateInclude.Day = true;
                    dateInclude.Hour = true;
                    dateInclude.Minute = false;
                    dateInclude.Second = false;
                    break;
                case ReportTimeStepFrequency.Daily:
                    dateInclude.Year = true;
                    dateInclude.Month = true;
                    dateInclude.Day = true;
                    dateInclude.Hour = false;
                    dateInclude.Minute = false;
                    dateInclude.Second = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(step), step, null);
            }

            return dateInclude;
        }
    }
}