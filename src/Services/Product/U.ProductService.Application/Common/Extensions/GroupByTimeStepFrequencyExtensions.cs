using System;
using U.ProductService.Application.Products.Models;
using U.ProductService.Application.Products.Queries.GetStatistics;

namespace U.ProductService.Application.Common.Extensions
{
    public static class GroupByTimeStepFrequencyExtensions
    {
        public static DateTimePropertiesInclude InitializeDtIncludes(this GroupByTimeStepFrequency step)
        {
            DateTimePropertiesInclude dateInclude = new DateTimePropertiesInclude();
            switch (step)
            {
                case GroupByTimeStepFrequency.Hourly:
                    dateInclude.Year = true;
                    dateInclude.Month = true;
                    dateInclude.Day = true;
                    dateInclude.Hour = true;
                    dateInclude.Minute = false;
                    dateInclude.Second = false;
                    break;
                case GroupByTimeStepFrequency.Daily:
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