using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services
{
    public interface IAuthenticate
    {
        Task<MobileServiceUser> Authenticate();
    }
}