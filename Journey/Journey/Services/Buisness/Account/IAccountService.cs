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

        Task<MobileServiceUser> AutehticateAsync();

        Task<Models.Account.Account> GetAccountAsync(bool sync = false);

        bool IsAccountAuthenticated();

        Task<bool> LoginFirstAsync();

        Task<bool> LogoutAsync();

        Task<Models.Account.Account> SaveAccountAsync(Models.Account.Account account, bool add);

        Task<bool> SoicalLoginAndSaveAsync(MobileServiceClient client);
    }
}