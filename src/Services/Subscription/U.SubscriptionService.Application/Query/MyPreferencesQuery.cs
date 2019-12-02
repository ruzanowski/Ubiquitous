using System;
using MediatR;
using U.Common.Subscription;

namespace U.SubscriptionService.Application.Query
{
    public class MyPreferencesQuery : IRequest<Preferences>
    {
        public Guid UserId { get; set; }
    }
}