using System;

namespace U.ProductService.Application.Common.Exceptions
{
    [Serializable]
    public class ProductServiceApplicationBaseException : SystemException
    {
        public ProductServiceApplicationBaseException() { }
        public ProductServiceApplicationBaseException(string message) : base(message) { }
        public ProductServiceApplicationBaseException(string message, Exception inner) : base(message, inner) { }
        
        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected ProductServiceApplicationBaseException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}