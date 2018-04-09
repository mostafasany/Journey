using System.Threading.Tasks;
using Journey.Models.Account;

namespace Journey.Services.Buisness.Goal.Data
{
    public interface IAccountGoalDataService
    {
        Task<AccountGoal> AddUpdateAccountGoalAsync(AccountGoal bodyWeight);
        Task<AccountGoal> GetAccountGoalAsync(bool sync = false);
    }
}