using System;

namespace Exceptions
{
    public class BuisnessException : Exception
    {
        public BuisnessException(string message) : base(message)
        {
        }

        public BuisnessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}