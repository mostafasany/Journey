using System.Threading.Tasks;

namespace Journey.Services.Buisness.Account.Data
{
    public interface IAccountDataService
    {
        Task<Tawasol.Models.Account> GetAccountAsync(bool sync = false);
        Task<Tawasol.Models.Account> AddUpdateAccountAsync(Tawasol.Models.Account account);
        bool IsAccountAuthenticated();
    }
}
