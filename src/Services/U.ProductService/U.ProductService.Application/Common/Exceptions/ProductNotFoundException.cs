using System;

namespace U.ProductService.Application.Common.Exceptions
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

        protected ProductNotFoundException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {

        }

        public ProductNotFoundException()
        {
        }
    }
}