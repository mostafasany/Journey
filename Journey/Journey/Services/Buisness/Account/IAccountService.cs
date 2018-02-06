using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Account
{
    public interface IAccountService
    {
        string Token { get; set; }

        Tawasol.Models.Account LoggedInAccount { get; set; }

        Task<Tawasol.Models.Account> SaveAccountAsync(Tawasol.Models.Account account);

        Task<Tawasol.Models.Account> GetAccountAsync(bool sync = false);

        Task<bool> LoginFirstAsync();

        Task<bool> LoginAsync(MobileServiceClient client);

        bool IsAccountAuthenticated();
    }
}