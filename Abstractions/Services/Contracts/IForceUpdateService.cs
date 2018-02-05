using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IForceUpdateService
    {
        string ForceUpdatePageKey { get; set; }
        string API { get; set; }
        Task<bool> CheckForceUpdateAsync();
    }
}