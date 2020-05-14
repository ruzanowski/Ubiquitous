namespace U.Common.Subscription
{
    public enum IntegrationEventType
    {
        Unknown,
        ProductPublishedIntegrationEvent,
        ProductPropertiesChangedIntegrationEvent,
        ProductAddedIntegrationEvent,
        NewProductFetched,
        UserConnected,
        UserDisconnected,
        AccessTokenRefreshedIntegrationEvent,
        SignedUp,
        SignedIn
    }
}