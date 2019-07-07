using System;

namespace U.FetchService.Application.Exceptions
{
    public class ZeroProductsFetchedException : Exception
    {
        public ZeroProductsFetchedException()
        {
        }

        public ZeroProductsFetchedException(string message)
            : base(message)
        {
        }

        public ZeroProductsFetchedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}