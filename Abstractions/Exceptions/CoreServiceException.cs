using System;

namespace Exceptions
{
    public class CoreServiceException : Exception
    {
        public CoreServiceException(string message) : base(message)
        {
        }

        public CoreServiceException(Exception innerException) : base(innerException.Message, innerException)
        {
        }

        public CoreServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}