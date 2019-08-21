using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Application.Products.Queries.QueryProducts;
using U.ProductService.Domain;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Products.Queries.QueryStatistics
{
    public partial class GetProductsStatisticsQueryHandler : IRequestHandler<GetProductsStatisticsQuery, IList<ProductStatisticsDto>>
    {
        private readonly ProductContext _context;

        private DateTimePropertiesInclude InitializeDtIncludes(ReportTimeStepFrequency step)
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

        public GetProductsStatisticsQueryHandler(ProductContext context)
        {
            _context = context;
        }

        public async Task<IList<ProductStatisticsDto>> Handle(GetProductsStatisticsQuery request, CancellationToken cancellationToken)
        {
            var dateInclude = InitializeDtIncludes(request.StepFrequency);
            var includeDescription = false;
            var query = GetQuery();

            var results = await query
                .GroupBy(g =>
                    new
                    {
                        g.CreatedAt.Year, // year exists in every query
                        Month = dateInclude.Month
                            ? g.CreatedAt.Month
                            : default,
                        Day = dateInclude.Day
                            ? g.CreatedAt.Day
                            : default,
                        Hour = dateInclude.Hour
                            ? g.CreatedAt.Hour
                            : default,
                        Minute = dateInclude.Minute
                            ? g.CreatedAt.Minute
                            : default,
                        Second = dateInclude.Second
                            ? g.CreatedAt.Second
                            : default,
                        Description = includeDescription
                            ? g.Description
                            : default
                    })
                .Select(i=> new
                {
                    i.Key.Description,
                    i.Key.Year,
                    i.Key.Month,
                    i.Key.Day,
                    i.Key.Hour,
                    i.Key.Minute,
                    i.Key.Second,
                    Count = i.Count()
                }).ToListAsync(cancellationToken);

            var mapped = results.Select(i => new ProductStatisticsDto
            {
                Description = i.Description,
                DateTime = DateTimeBuilder(i.Year, i.Month, i.Day, i.Hour, i.Minute, i.Second),
                Count = i.Count
            }).ToList();

            return mapped;
        }

        private DateTime DateTimeBuilder(int year, int month, int day, int hour, int minute, int second)
        {
            month = month is 0 ? 1 : month;
            day = day is 0 ? 1 : day;
            hour = hour is 0 ? 12 : hour;
            return new DateTime(year, month, day, hour, minute, second);
        }
        
        private IQueryable<Product> GetQuery()
        {
            return _context.Products
                .Include(x => x.Pictures)
                .AsQueryable();
        }
    }
}