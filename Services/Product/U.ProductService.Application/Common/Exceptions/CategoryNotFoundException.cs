using System;

namespace U.ProductService.Application.Exceptions
{
    [Serializable]
    public class CategoryNotFoundException : ProductServiceApplicationBaseException
    {
        public CategoryNotFoundException(string message)
            : base(message)
        {
        }

        public CategoryNotFoundException(string message, Exception inner): base(message, inner)
        {
        }   
    }
}