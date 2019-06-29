namespace U.SmartStoreAdapter.IntegrationTests
{
    public static class EndpointsAddresses
    {
        private const string EndpointPrefix = "api/smartstore";
        public static class ProductsAddresses
        {
            private const string ProductPrefix = "products";
            private const string GetListSuffix = "get-list";

            public static readonly string GetListEndpoint = $"/{EndpointPrefix}/{ProductPrefix}/{GetListSuffix}";
        }
        // further endpoints. Not done.
    }
}