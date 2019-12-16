using System.Collections.Generic;
using MediatR;

namespace U.NotificationService.Application.Queries.GetTypesCount
{
    public class GetNotificationTypesCount : IRequest<IEnumerable<NotificationTypesCount>>
    {

    }
}