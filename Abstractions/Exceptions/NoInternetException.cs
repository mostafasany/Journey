using System;

namespace Abstractions.Exceptions
{
    public class NoInternetException : Exception
    {
        public NoInternetException() : base("No Internet connection")
        {
        }

        public NoInternetException(Exception innerException) : base("No Internet connection", innerException)
        {
        }
    }
}