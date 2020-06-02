using System;

namespace U.ProductService.Application.Common.Exceptions
{
    [Serializable]
    public class ProductCategoryNotFoundException : ProductServiceApplicationBaseException
    {
        public ProductCategoryNotFoundException(string message)
            : base(message)
        {
        }

        public ProductCategoryNotFoundException(string message, Exception inner): base(message, inner)
        {
        }   
    }
}