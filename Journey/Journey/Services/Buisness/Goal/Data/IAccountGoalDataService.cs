using Journey.Models.Account;
using System.Threading.Tasks;

namespace Tawasol.Services.Data
{
    public interface IAccountGoalDataService
    {
        Task<AccountGoal> GetAccountGoalAsync(bool sync = false);
        Task<AccountGoal> AddUpdateAccountGoalAsync(AccountGoal bodyWeight);
    }
}
