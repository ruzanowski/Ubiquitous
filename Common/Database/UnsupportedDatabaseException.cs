using System;

namespace U.SmartStoreAdapter.Application.Models.Exceptions
{
    public class UnsupportedDatabaseException : Exception
    {
        public UnsupportedDatabaseException(string message)
            : base(message)
        {
        }
    }
}