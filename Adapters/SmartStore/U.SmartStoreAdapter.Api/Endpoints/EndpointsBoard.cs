namespace U.SmartStoreAdapter.Api.Endpoints
{
    public static class EndpointsBoard
    {
        private static string BaseUrl { get;  } = "http://localhost";
        public static int Port { get;  } = 5000;
        private static string EndpointBase { get; } = "/api/smartstore/";
        private static string Products { get; } = "products";
        private static string Categories { get; } = "categories";
        private static string Pictures { get; } = "pictures";
        private static string TaxCategory { get; } = "taxcategory";
        private static string Manufacturers { get; } = "manufacturers";

        public static string ProductEndpoint(int port)
        {
            return $"{BaseUrl}:{port}{EndpointBase}{Products}/get-list";
        }
        public static string CategoriesEndpoint(int port)
        {
            return $"{BaseUrl}:{port}{EndpointBase}{Categories}/get-list";
        }
        public static string PicturesEndpoint(int port)
        {
            return $"{BaseUrl}:{port}{EndpointBase}{Pictures}/get-list";
        }
        public static string TaxCategoryEndpoint(int port)
        {
            return $"{BaseUrl}:{port}{EndpointBase}{TaxCategory}/get-list";
        }
        public static string ManufacturersEndpoint(int port)
        {
            return $"{BaseUrl}:{port}{EndpointBase}{Manufacturers}/get-list";
        }
    }
}