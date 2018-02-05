using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface ISettingsService
    {
        Task<string> Get(string key);
        Task Set(string key, string value);
        Task Remove(string key);
    }
}