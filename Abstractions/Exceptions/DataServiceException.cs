using System;

namespace Exceptions
{
    public class DataServiceException : Exception
    {
        public DataServiceException(string message) : base(message)
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