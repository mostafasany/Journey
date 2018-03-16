using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IPlatformService
    {
        string StoreId { get; set; }
        string GetAppVersionAsync();
        Task<bool> OpenStoreAsync();
    }
}