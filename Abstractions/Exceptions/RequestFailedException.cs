using System;

namespace Exceptions
{
    public class RequestFailedException : Exception
    {
        public RequestFailedException(string message) : base(message)
        {
        }

        public RequestFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}