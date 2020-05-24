using System;

namespace U.Common.NetCore.EF
{
    public class UnsupportedDatabaseException : Exception
    {
        public UnsupportedDatabaseException(string message)
            : base(message)
        {
        }

        public UnsupportedDatabaseException()
        {
        }

        public UnsupportedDatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}