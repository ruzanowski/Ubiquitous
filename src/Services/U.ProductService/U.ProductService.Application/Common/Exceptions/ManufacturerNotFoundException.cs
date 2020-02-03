using System;

namespace U.ProductService.Application.Common.Exceptions
{
    [Serializable]
    public class ManufacturerNotFoundException : ProductServiceApplicationBaseException
    {
        public ManufacturerNotFoundException(string message)
            : base(message)
        {
        }

        public ManufacturerNotFoundException(string message, Exception inner): base(message, inner)
        {
        }   
    }
}