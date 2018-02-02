using System.Threading.Tasks;

namespace Services.Core
{
    public interface IPlatformService
    {
        string StoreId { get; set; }
        Task<bool> OpenStoreAsync();
        string GetAppVersionAsync();
    }
}