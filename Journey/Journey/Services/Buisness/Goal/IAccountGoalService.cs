using System.Threading.Tasks;
using Journey.Models.Account;

namespace Journey.Services.Buisness.Goal
{
    public interface IAccountGoalService
    {
        Task<AccountGoal> AddAccountGoal(AccountGoal goal);
        Task<AccountGoal> GetAccountGoalAsync(bool sync = false);
    }
}