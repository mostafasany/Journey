using System;
using System.Collections.Generic;
using Abstractions.Services.Contracts;
using Prism.Logging;

namespace Journey.Services.Forms
{
    public class LoggerService : ILoggerService
    {
        public void Log(string message, Category category, Priority priority)
        {
        }

        public void Log(object obj, Category category, Priority priority, IDictionary<string, string> properties = null)
        {
        }

        public void LogException(Exception ex, IDictionary<string, string> properties = null)
        {
        }

        public void LogPageView(string pageName, IDictionary<string, string> properties = null)
        {
        }
    }
}