using System;

namespace U.FetchService.Application.Exceptions
{
    public class FetchFailedException : Exception
    {
        public FetchFailedException()
        {
        }

        public FetchFailedException(string message)
            : base(message)
        {
        }

        public FetchFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}