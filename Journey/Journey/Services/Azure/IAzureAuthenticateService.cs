using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Azure
{
    public interface IAzureAuthenticateService
    {
        Task<MobileServiceUser> AuthenticateAsync();
        Task<bool> LogoutAsync();
    }
}