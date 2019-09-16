using System;

namespace U.Common.Database
{
    public class UnsupportedDatabaseException : Exception
    {
        public UnsupportedDatabaseException(string message)
            : base(message)
        {
        }
    }
}