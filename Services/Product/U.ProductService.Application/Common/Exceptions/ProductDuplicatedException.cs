using System;

namespace U.ProductService.Application.Exceptions
{
    [Serializable]
    public class ProductDuplicatedException : ProductServiceApplicationBaseException
    {
        public ProductDuplicatedException(string message)
            : base(message)
        {
        }

        public ProductDuplicatedException(string message, Exception inner): base(message, inner)
        {
        }   
    }
}