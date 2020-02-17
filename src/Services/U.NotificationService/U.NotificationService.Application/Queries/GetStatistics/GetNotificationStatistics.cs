using System.Collections.Generic;
using MediatR;

namespace U.NotificationService.Application.Queries.GetStatistics
{
    public class GetNotificationStatistics : IRequest<IEnumerable<NotificationStatistics>>
    {

    }
}