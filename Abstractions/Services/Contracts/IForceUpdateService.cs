using System.Threading.Tasks;

namespace Services.Core
{
    public interface IForceUpdateService
    {
        string ForceUpdatePageKey { get; set; }
        string API { get; set; }
        Task<bool> CheckForceUpdateAsync();
    }
}