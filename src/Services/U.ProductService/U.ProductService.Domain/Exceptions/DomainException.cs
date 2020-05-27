using System;

namespace U.ProductService.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message)
            : base(message)
        { }

    }
}
