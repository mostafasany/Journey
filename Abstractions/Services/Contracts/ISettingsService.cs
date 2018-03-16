using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface ISettingsService
    {
        Task<string> Get(string key);
        Task Remove(string key);
        Task Set(string key, string value);
    }
}