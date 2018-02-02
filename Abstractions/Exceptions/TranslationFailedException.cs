using System;

namespace Exceptions
{
    public class TranslationFailedException : Exception
    {
        public TranslationFailedException(string message) : base(message)
        {
        }

        public TranslationFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}