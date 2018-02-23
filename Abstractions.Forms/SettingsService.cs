using System;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Abstractions.Services.Contracts;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Journey.Services.Forms
{
    public class SettingsService : ISettingsService
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public async Task<string> Get(string key)
        {
            try
            {
                return AppSettings.GetValueOrDefault(key, string.Empty);
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message);
            }
        }

        public async Task Set(string key, string value)
        {
            try
            {
                AppSettings.AddOrUpdateValue(key, value);
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message);
            }
        }

        public async Task Remove(string key)
        {
            try
            {
                AppSettings.Remove(key);
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message);
            }
        }
    }
}