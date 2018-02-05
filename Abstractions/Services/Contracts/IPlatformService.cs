using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IPlatformService
    {
        string StoreId { get; set; }
        Task<bool> OpenStoreAsync();
        string GetAppVersionAsync();
    }
}