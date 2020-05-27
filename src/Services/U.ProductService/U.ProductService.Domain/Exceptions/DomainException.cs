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

    public class NotFoundDomainException : DomainException
    {
        public NotFoundDomainException(string message)
            : base(message)
        {
        }
    }
}
