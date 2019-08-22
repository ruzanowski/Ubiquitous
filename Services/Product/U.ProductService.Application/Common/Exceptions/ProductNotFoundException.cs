using System;

namespace U.ProductService.Application.Exceptions
{
    [Serializable]
    public class ProductNotFoundException : ProductServiceApplicationBaseException
    {
        public ProductNotFoundException(string message)
            : base(message)
        {
        }

        public ProductNotFoundException(string message, Exception inner): base(message, inner)
        {
        }   
    }
}