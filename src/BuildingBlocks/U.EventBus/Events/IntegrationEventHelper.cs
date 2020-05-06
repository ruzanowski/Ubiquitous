using System;
using System.Collections.Generic;
using U.EventBus.Events.Fetch;
using U.EventBus.Events.Identity;
using U.EventBus.Events.Notification;
using U.EventBus.Events.Product;

namespace U.EventBus.Events
{
    public static class IntegrationEventHelper
    {
        public static List<Type> GetTypes()
        {
            return new List<Type>
            {
                typeof(NewProductFetchedIntegrationEvent),

                typeof(AccessTokenRefreshedIntegrationEvent),

                typeof(SignedInIntegrationEvent),

                typeof(SignedUpIntegrationEvent),

                typeof(UserConnectedIntegrationEvent),
                typeof(UserConnectedEmailIntegrationEvent),
                typeof(UserConnectedSignalRIntegrationEvent),

                typeof(UserDisconnectedIntegrationEvent),
                typeof(UserDisconnectedEmailIntegrationEvent),
                typeof(UserDisconnectedSignalRIntegrationEvent),

                typeof(ProductAddedIntegrationEvent),
                typeof(ProductAddedEmailIntegrationEvent),
                typeof(ProductAddedSignalRIntegrationEvent),

                typeof(ProductPropertiesChangedIntegrationEvent),
                typeof(ProductPropertiesChangedEmailIntegrationEvent),
                typeof(ProductPropertiesChangedSignalRIntegrationEvent),

                typeof(ProductPublishedIntegrationEvent),
                typeof(ProductPublishedEmailIntegrationEvent),
                typeof(ProductPublishedSignalRIntegrationEvent),

                typeof(IntegrationEvent)
            };
        }
    }
}