using System.Threading.Tasks;
using Abstractions.Services.Contracts;

namespace Journey.Services.Forms
{
    public class SettingsService : ISettingsService
    {
        public async Task<string> Get(string key)
        {
            return null;
            // return AppSettings.GetValueOrDefault<string>(key);
        }

        public async Task Set(string key, string value)
        {
            // AppSettings.AddOrUpdateValue(key, value);
        }

        public async Task Remove(string key)
        {
            // AppSettings.Remove(key);
        }
        //private static ISettings AppSettings
        //{
        //    get
        //    {
        //        return CrossSettings.Current;
        //    }
        //}
    }
}