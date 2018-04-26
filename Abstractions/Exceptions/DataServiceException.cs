using System;

namespace Abstractions.Exceptions
{
    public class DataServiceException : Exception
    {
        public DataServiceException(Type exceptionType, Exception innerException) : base(innerException.Message, innerException)
        {
        }

        public DataServiceException(Exception innerException) : base(innerException.Message, innerException)
        {
        }

        public DataServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}