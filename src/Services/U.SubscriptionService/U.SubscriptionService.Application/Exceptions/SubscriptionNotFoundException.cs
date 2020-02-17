using System;

namespace U.SubscriptionService.Application.Exceptions
{
    public class SubscriptionNotFoundException : Exception
    {
        public SubscriptionNotFoundException()
        {
        }

        public SubscriptionNotFoundException(string message)
            : base(message)
        {
        }

        public SubscriptionNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}