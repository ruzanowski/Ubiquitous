namespace U.SmartStoreAdapter.Api.Endpoints
{
    public static class EndpointsBoard
    {
        private static string EndpointBase { get; } = "/api/smartstore/";
        private static string Products { get; } = "products";

        public static string GetProductListUrl(string baseUrl, int port) =>
            $"{GetCorrectUrl(baseUrl)}:{port}{EndpointBase}{Products}/get-list";

        private static string GetCorrectUrl(string baseUrl) =>
            $"{(baseUrl.StartsWith("http://") ? baseUrl : $"http://{baseUrl}")}";
    }
}