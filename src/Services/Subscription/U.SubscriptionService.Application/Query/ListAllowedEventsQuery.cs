using System;
using System.Collections.Generic;
using MediatR;
using U.NotificationService.Domain.Entities;

namespace U.SubscriptionService.Application.Query
{
    public class ListAllowedEventsQuery : IRequest<IList<string>>
    {
        public Guid UserId { get; set; }

    }
}