using System;

namespace U.ProductService.IntegrationTests
{
    public partial class TestBase
    {
        protected static class ProductController
        {
            private const string BaseUrl = "/api/product/products";
            public static string GetProducts() => $"{BaseUrl}";
            public static string CreateProduct() => $"{BaseUrl}/create";
            public static string GetProduct(Guid id) => $"{BaseUrl}/{id}";
            public static string UpdateProduct() => $"{BaseUrl}";
            public static string PublishProduct(Guid id) => $"{BaseUrl}/{id}/publish";
            public static string UnpublishProduct(Guid id) => $"{BaseUrl}/{id}/unpublish";
            public static string ChangePriceProduct(Guid id, int price) => $"{BaseUrl}/{id}/price?price={price}";
            public static string AttachPictureToProduct(Guid id, Guid pictureId) => $"{BaseUrl}/{id}/picture/{pictureId}";
            public static string DetachPictureFromProduct(Guid id, Guid pictureId) => $"{BaseUrl}/{id}/picture/{pictureId}";
            public static string ProductStatisticsByManufacturer() => $"{BaseUrl}/statistics/manufacturer";
            public static string ProductStatisticsByCategory() => $"{BaseUrl}/statistics/category";
            public static string ProductStatisticsByCreation() => $"{BaseUrl}/statistics/creation";
            public static string ProductCount() => $"{BaseUrl}/count";

        }

        protected static class PicturesController
        {
            private const string BaseUrl = "/api/product/pictures";
            public static string AddPicture() => $"{BaseUrl}";
            public static string DeletePicture(Guid id) => $"{BaseUrl}/{id}";
        }
    }
}