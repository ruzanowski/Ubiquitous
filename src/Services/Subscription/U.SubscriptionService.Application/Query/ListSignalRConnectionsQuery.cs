using System;
using System.Collections.Generic;
using MediatR;

namespace U.SubscriptionService.Application.Query
{
    public class ListSignalRConnectionQuery : IRequest<IList<string>>
    {
        public Guid UserId { get; set; }
    }
}