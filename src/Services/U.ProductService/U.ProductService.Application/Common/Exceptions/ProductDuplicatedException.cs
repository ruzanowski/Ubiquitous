using System;

namespace U.ProductService.Application.Common.Exceptions
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

        protected ProductDuplicatedException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
        }

        public ProductDuplicatedException()
        {
        }
    }
}