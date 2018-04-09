using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Contracts;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Account.Data
{
    public interface IAccountDataService
    {
        Task<Models.Account.Account> AddUpdateAccountAsync(Models.Account.Account account, bool add);
        Task<MobileServiceUser> AutehticateAsync();
        Task<Models.Account.Account> GetAccountAsync(bool sync = false);
        bool IsAccountAuthenticated();
        Task<List<Social>> MeAsync();
    }
}