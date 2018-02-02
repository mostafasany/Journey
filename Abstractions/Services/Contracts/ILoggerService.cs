using System;
using System.Collections.Generic;
using Prism.Logging;

namespace Services.Core
{
    public interface ILoggerService : ILoggerFacade
    {
        void Log(object obj, Category category, Priority priority, IDictionary<string, string> properties = null);
        void LogException(Exception ex, IDictionary<string, string> properties = null);
        void LogPageView(string pageName, IDictionary<string, string> properties = null);
    }
}