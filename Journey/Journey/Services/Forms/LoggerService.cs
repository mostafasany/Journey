using System;
using System.Collections.Generic;
using Abstractions.Exceptions;
using Abstractions.Services.Contracts;
using Prism.Logging;

namespace Journey.Services.Forms
{
    public class LoggerService : ILoggerService
    {
        public void Log(string message, Category category, Priority priority)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message);
            }
        }

        public void Log(object obj, Category category, Priority priority, IDictionary<string, string> properties = null)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message);
            }
        }

        public void LogException(Exception ex, IDictionary<string, string> properties = null)
        {
            try
            {

            }
            catch (Exception e)
            {
                throw new CoreServiceException(e.Message);
            }
        }

        public void LogPageView(string pageName, IDictionary<string, string> properties = null)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message);
            }
        }
    }
}