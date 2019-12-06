using System;
using MediatR;

namespace U.SubscriptionService.Application.Command.Preferences
{
    public class SetPreferencesCommand :  IRequest
    {
        public Guid UserId { get; set; }
        public Common.Subscription.Preferences Preferences { get; set; }
    }
}