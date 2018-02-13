using System;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Account;
using Tawasol.Services.Data;

namespace Journey.Services.Buisness.Goal
{
    public class AccountGoalService : IAccountGoalService
    {
        private readonly IAccountGoalDataService accountDataService;

        public AccountGoalService(IAccountGoalDataService _accountDataService)
        {
            accountDataService = _accountDataService;
        }

        public async Task<AccountGoal> AddAccountGoal(AccountGoal goal)
        {
            try
            {
                var accountGoal = new AccountGoal();
                accountGoal = await accountDataService.AddUpdateAccountGoalAsync(goal);
                return accountGoal;
            }

            catch (Exception ex)
            {
                throw new BuisnessException(ex.Message, ex);
            }
        }

        public async Task<AccountGoal> GetAccountGoalAsync(bool sync = false)
        {
            try
            {
                var accountGoals = await accountDataService.GetAccountGoalAsync(sync);
                return accountGoals;
            }

            catch (Exception ex)
            {
                throw new BuisnessException(ex.Message, ex);
            }
        }
    }
}