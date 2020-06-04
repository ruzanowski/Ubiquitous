using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using U.NotificationService.Domain.Entities;
using U.NotificationService.Infrastructure.Contexts;

namespace U.NotificationService.Application.Queries.GetStatistics
{
    public class GetNotificationStatisticsQueryHandler : IRequestHandler<GetNotificationStatistics,
        IEnumerable<NotificationStatistics>>
    {
        private readonly NotificationContext _context;
        private const int Days = 14;

        public GetNotificationStatisticsQueryHandler(NotificationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NotificationStatistics>> Handle(GetNotificationStatistics request,
            CancellationToken cancellationToken)
        {
            var notifications = GetQuery();

            var groupedStatistics = notifications
                .Where(x => x.CreationDate.Date.AddDays(Days) >= DateTime.UtcNow.Date)
                .GroupBy(g =>
                    new
                    {
                        g.CreationDate.Year,
                        g.CreationDate.Month,
                        g.CreationDate.Day
                    })
                .Select(i => new
                {
                    i.Key.Year,
                    i.Key.Month,
                    i.Key.Day,
                    Count = i.Sum(x => x.TimesSent)
                }).ToList();

            var statisticsWithGaps = groupedStatistics
                .Select(x => new NotificationStatistics
                {
                    DateTime = new DateTime(x.Year, x.Month, x.Day),
                    Count = x.Count
                }).ToList();

            var missingDateTimes = GetMissingDateTimes();

            return FulfillGaps(statisticsWithGaps, missingDateTimes);
        }

        private IQueryable<Notification> GetQuery() => _context.Notifications
            .AsQueryable();

        private IList<NotificationStatistics> GetMissingDateTimes()
        {
            var dateTimes = new List<DateTime>();
            for (var i = 0; i < Days; i++)
            {
                dateTimes.Add(DateTime.Now.AddDays(-i).Date);
            }

            var missingDateTimes = dateTimes.Select(x => new NotificationStatistics
            {
                Count = 0,
                DateTime = x
            });

            return missingDateTimes.ToList();
        }


        private IList<NotificationStatistics> FulfillGaps(IList<NotificationStatistics> statisticsWithGaps,
            IList<NotificationStatistics> fillers)
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
    }
}