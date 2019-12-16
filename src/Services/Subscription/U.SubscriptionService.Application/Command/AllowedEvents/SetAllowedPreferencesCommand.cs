using System;
using System.Collections.Generic;
using MediatR;
using U.Common.Subscription;

namespace U.SubscriptionService.Application.Command.AllowedEvents
{
    public class SetAllowedPreferencesCommand : IRequest
    {
        public Guid UserId { get; set; }
        public ISet<IntegrationEventType> IntegrationEventTypes { get; set; }
    }
}