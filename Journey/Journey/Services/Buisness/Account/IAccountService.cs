using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Account
{
    public interface IAccountService
    {
        string AccountTokenKey { get; }
        string AccountIdKey { get; }
        string Token { get; set; }

        Models.Account.Account LoggedInAccount { get; set; }

        Task<Models.Account.Account> SaveAccountAsync(Models.Account.Account account, bool add);

        Task<Models.Account.Account> GetAccountAsync(bool sync = false);

        Task<bool> LoginFirstAsync();

        Task<MobileServiceUser> AutehticateAsync();

        Task<bool> SoicalLoginAndSaveAsync(MobileServiceClient client);

        Task<bool> LogoutAsync();

        bool IsAccountAuthenticated();
    }
}