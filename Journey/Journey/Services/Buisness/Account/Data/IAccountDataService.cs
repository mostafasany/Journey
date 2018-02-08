using System.Threading.Tasks;

namespace Journey.Services.Buisness.Account.Data
{
    public interface IAccountDataService
    {
        Task<Models.Account.Account> GetAccountAsync(bool sync = false);
        Task<Models.Account.Account> AddUpdateAccountAsync(Models.Account.Account account,bool add);
        bool IsAccountAuthenticated();
    }
}