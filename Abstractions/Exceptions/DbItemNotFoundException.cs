using System;

namespace Abstractions.Exceptions
{
    public class DbItemNotFoundException : Exception
    {
        public DbItemNotFoundException(string message) : base(message)
        {
        }

        public DbItemNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}