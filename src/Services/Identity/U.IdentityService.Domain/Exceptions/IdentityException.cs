using System;

namespace U.IdentityService.Domain.Exceptions
{
    public class IdentityException : Exception
    {
        public string Code { get; set; }

        public IdentityException(string code, string message, params object[] args)
            : this(null, code, message, args)
        {
        }

        private IdentityException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}