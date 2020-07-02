using System;

namespace U.ProductService.IntegrationTests
{
    public partial class TestBase
    {
        protected static class CategoryController
        {
            private const string BaseUrl = "/api/product/categories";
            public static string GetList() => $"{BaseUrl}";
            public static string Create() => $"{BaseUrl}";
            public static string Get(Guid id) => $"{BaseUrl}/{id}";
            public static string Update() => $"{BaseUrl}";
            public static string Count() => $"{BaseUrl}/count";
        }

        protected static class ManufacturerController
        {
            private const string BaseUrl = "/api/product/manufacturers";
            public static string GetList() => $"{BaseUrl}";
            public static string Create() => $"{BaseUrl}";
            public static string Get(Guid id) => $"{BaseUrl}/{id}";
            public static string AttachPicture(Guid id, Guid pictureId) => $"{BaseUrl}/{id}/picture/{pictureId}";
            public static string DetachPicture(Guid id, Guid pictureId) => $"{BaseUrl}/{id}/picture/{pictureId}";
            public static string Count() => $"{BaseUrl}/count";
        }

        protected static class ProductController
        {
            private const string BaseUrl = "/api/product/products";
            public static string GetList() => $"{BaseUrl}";
            public static string Create() => $"{BaseUrl}";
            public static string Get(Guid id) => $"{BaseUrl}/{id}";
            public static string Update() => $"{BaseUrl}";
            public static string Publish(Guid id) => $"{BaseUrl}/{id}/publish";
            public static string Unpublish(Guid id) => $"{BaseUrl}/{id}/unpublish";
            public static string ChangePrice(Guid id, int price) => $"{BaseUrl}/{id}/price?price={price}";
            public static string AttachPicture(Guid id, Guid pictureId) => $"{BaseUrl}/{id}/picture/{pictureId}";
            public static string DetachPicture(Guid id, Guid pictureId) => $"{BaseUrl}/{id}/picture/{pictureId}";
            public static string StatisticsByManufacturer() => $"{BaseUrl}/statistics/manufacturer";
            public static string StatisticsByCategory() => $"{BaseUrl}/statistics/category";
            public static string StatisticsByCreation() => $"{BaseUrl}/statistics/creation";
            public static string Count() => $"{BaseUrl}/count";
        }

        protected static class PicturesController
        {
            private const string BaseUrl = "/api/product/pictures";
            public static string GetPicture(Guid id) => $"{BaseUrl}/{id}";
            public static string AddPicture() => $"{BaseUrl}";
            public static string DeletePicture(Guid id) => $"{BaseUrl}/{id}";
            public static string UpdatePicture(Guid id) => $"{BaseUrl}/{id}";
        }
    }
}