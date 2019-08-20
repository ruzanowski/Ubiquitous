using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.ProductService.Application.Products.Queries.QueryProducts;
using U.ProductService.Domain.Aggregates;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Application.Products.Queries.QueryStatistics
{
    public class GetProductsStatisticsQueryHandler : IRequestHandler<GetProductsStatisticsQuery, IList<ProductStatisticsDto>>
    {
        private readonly ProductContext _context;
        private DateTimePropertiesInclude _dateInclude = new DateTimePropertiesInclude();
        private PropertiesInclude _propertiesInclude = new PropertiesInclude();

        private class DateTimePropertiesInclude
        {
            public bool Year { get; set; }
            public bool Month { get; set; }
            public bool Day { get; set; }
            public bool Hour { get; set; }
            public bool Minute { get; set; }
            public bool Second { get; set; }
        }

        private class PropertiesInclude
        {
            public bool Description { get; set; }
        }

        private void InitializeDtIncludes(ReportTimeStepFrequency step)
        {
            switch (step)
            {
                case ReportTimeStepFrequency.Secondly:
                    _dateInclude.Year = true;
                    _dateInclude.Month = true;
                    _dateInclude.Day = true;
                    _dateInclude.Hour = true;
                    _dateInclude.Minute = true;
                    _dateInclude.Second = true;
                    break;
                case ReportTimeStepFrequency.Minutely:
                    _dateInclude.Year = true;
                    _dateInclude.Month = true;
                    _dateInclude.Day = true;
                    _dateInclude.Hour = true;
                    _dateInclude.Minute = true;
                    _dateInclude.Second = false;
                    break;
                case ReportTimeStepFrequency.Hourly:
                    _dateInclude.Year = true;
                    _dateInclude.Month = true;
                    _dateInclude.Day = true;
                    _dateInclude.Hour = true;
                    _dateInclude.Minute = false;
                    _dateInclude.Second = false;
                    break;
                case ReportTimeStepFrequency.Daily:
                    _dateInclude.Year = true;
                    _dateInclude.Month = true;
                    _dateInclude.Day = true;
                    _dateInclude.Hour = false;
                    _dateInclude.Minute = false;
                    _dateInclude.Second = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(step), step, null);
            }
        }

        private void InitializePropertiesIncludes()
        {
            _propertiesInclude.Description = true;
        }

        public GetProductsStatisticsQueryHandler(ProductContext context)
        {
            _context = context;
        }

        public async Task<IList<ProductStatisticsDto>> Handle(GetProductsStatisticsQuery request, CancellationToken cancellationToken)
        {

            InitializeDtIncludes(request.StepFrequency);
            InitializePropertiesIncludes();
            var query = GetQuery();

            var results = query
                .GroupBy(g =>
                    new
                    {
                        //the trick is with includes, to avoid writing complicated group by
                        //we point always on column that we will be grouping by.
                        //e.g. if "Day" is not included, then we will group twice by "Year".
                        
                        g.CreatedAt.Year, // year exists in every query
                        Month = _dateInclude.Month ? g.CreatedAt.Month : g.CreatedAt.Year,
                        Day = _dateInclude.Day ? g.CreatedAt.Day : g.CreatedAt.Year,
                        Hour = _dateInclude.Hour  ? g.CreatedAt.Hour : g.CreatedAt.Year,
                        Minute = _dateInclude.Minute  ? g.CreatedAt.Minute : g.CreatedAt.Year,
                        Second = _dateInclude.Second  ? g.CreatedAt.Second : g.CreatedAt.Year,
                        Description = _propertiesInclude.Description? g.Description:g.CreatedAt.Year.ToString()
                    })
                .Select(i => new ProductStatisticsDto
                {
                    Description = i.Key.Description,
                    DateTime = DateTimeBuilder(i.Key.Year, i.Key.Month,i.Key.Day,i.Key.Hour, i.Key.Minute, i.Key.Second),
                    Count  = i.Count()
                }).ToList();

            return results;
        }

        private DateTime DateTimeBuilder(int year, int month, int day, int hour, int minute, int second)
        {
            month = month.Equals(year) ? 1 : month;
            day = day.Equals(year) ? 1 : day;
            hour = hour.Equals(year) ? 12 : hour;
            minute = minute.Equals(year) ? 0 : minute;
            second = second.Equals(year) ? 0 : second;
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