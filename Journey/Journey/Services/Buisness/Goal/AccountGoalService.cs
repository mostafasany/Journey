using System;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Account;
using Journey.Services.Buisness.Goal.Data;

namespace Journey.Services.Buisness.Goal
{
    public class AccountGoalService : IAccountGoalService
    {
        private readonly IAccountGoalDataService _accountDataService;

        public AccountGoalService(IAccountGoalDataService accountDataService)
        {
            _accountDataService = accountDataService;
        }

        public async Task<AccountGoal> AddAccountGoal(AccountGoal goal)
        {
            try
            {
                var accountGoal = await _accountDataService.AddUpdateAccountGoalAsync(goal);
                return accountGoal;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<AccountGoal> GetAccountGoalAsync(bool sync = false)
        {
            try
            {
                var accountGoals = await _accountDataService.GetAccountGoalAsync(sync);
                return accountGoals;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}