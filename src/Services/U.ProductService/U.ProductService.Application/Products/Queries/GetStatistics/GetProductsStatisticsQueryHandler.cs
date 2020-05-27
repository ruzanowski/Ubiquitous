using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Application.Common.Extensions;
using U.ProductService.Application.Products.Models;
using U.ProductService.Domain;
using U.ProductService.Domain.Aggregates.Product;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Products.Queries.GetStatistics
{
    public class GetProductsStatisticsQueryHandler : IRequestHandler<GetProductsStatisticsQuery, IList<ProductStatisticsDto>>
    {
        private readonly ProductContext _context;
        private const int Days = 14;

        public GetProductsStatisticsQueryHandler(ProductContext context)
        {
            _context = context;
        }

        public async Task<IList<ProductStatisticsDto>> Handle(GetProductsStatisticsQuery request, CancellationToken cancellationToken)
        {
            var dateInclude = request.StepFrequency.InitializeDtIncludes();
            var includeDescription = false;
            var query = GetQuery();

            var results = await query
                .Where(x => x.CreatedAt.Date.AddDays(Days) >= DateTime.UtcNow.Date)
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

            var statisticsWithGaps = results.Select(i => new ProductStatisticsDto
            {
                Description = i.Description,
                DateTime = DateTimeBuilder(i.Year, i.Month, i.Day, i.Hour, i.Minute, i.Second),
                Count = i.Count
            }).ToList();

            var fillingDays = GetZeroTimesForGaps();
            var statisticsWithoutGaps = FulfillGaps(statisticsWithGaps, fillingDays);

            return statisticsWithoutGaps;
        }

        private DateTime DateTimeBuilder(int year, int month, int day, int hour, int minute, int second)
        {
            month = month is 0 ? 1 : month;
            day = day is 0 ? 1 : day;
            hour = hour is 0 ? 12 : hour;
            return new DateTime(year, month, day, hour, minute, second);
        }

        private IList<ProductStatisticsDto> GetZeroTimesForGaps()
        {
            var dateTimes = new List<DateTime>();
            for (var i = 0; i < Days; i++)
            {
                dateTimes.Add(DateTime.Now.AddDays(-i).Date);
            }

            var missingDateTimes = dateTimes.Select(x => new ProductStatisticsDto
            {
                Count = 0,
                DateTime = x
            });

            return missingDateTimes.ToList();
        }

        private IList<ProductStatisticsDto> FulfillGaps(IList<ProductStatisticsDto> statisticsWithGaps,
            IList<ProductStatisticsDto> fillers)
        {
            foreach (var filler in fillers)
            {
                if (!statisticsWithGaps.Select(x => x.DateTime.Date).Contains(filler.DateTime.Date))
                {
                    statisticsWithGaps.Add(filler);
                }
            }

            var statisticsWithoutGaps = statisticsWithGaps
                .OrderBy(x => x.DateTime)
                .ToList();

            return statisticsWithoutGaps;
        }

        private IQueryable<Product> GetQuery()
        {
            return _context.Products
                .Include(x => x.Pictures)
                .AsQueryable();
        }
    }
}