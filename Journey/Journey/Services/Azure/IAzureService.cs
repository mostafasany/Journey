using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Azure
{
    public interface IAzureService
    {
        MobileServiceClient CreateOrGetAzureClient(string id, string token);
    }
}