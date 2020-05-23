using System;

namespace U.Common.NetCore.EF
{
    public class UnsupportedDatabaseException : Exception
    {
        public UnsupportedDatabaseException(string message)
            : base(message)
        {
        }
    }
}