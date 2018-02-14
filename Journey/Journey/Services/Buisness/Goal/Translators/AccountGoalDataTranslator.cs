using System;
using Abstractions.Exceptions;
using Journey.Models.Account;
using Journey.Services.Buisness.Goal.Dto;

namespace Journey.Services.Buisness.Goal.Translators
{
    public static class AccountGoalDataTranslator
    {
        public static AccountGoal TranslateAccountGoal(AzureAccountGoal accountGoal)
        {
            try
            {
                var accountGoalDto = new AccountGoal();
                if (accountGoal != null)
                {
                    accountGoalDto.Weight = accountGoal.Weight;
                    accountGoalDto.Goal = accountGoal.Goal;
                    accountGoalDto.Start = accountGoal.Start;
                    accountGoalDto.End = accountGoal.End;
                }

                return accountGoalDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Goal", ex.InnerException);
            }
        }


        public static AzureAccountGoal TranslateAccountGoal(AccountGoal accountGoal, string account)
        {
            try
            {
                var accountGoalDto = new AzureAccountGoal();
                if (accountGoal != null)
                {
                    accountGoalDto.Account = account;
                    accountGoalDto.Weight = accountGoal.Weight;
                    accountGoalDto.Goal = accountGoal.Goal;
                    accountGoalDto.Start = accountGoal.Start;
                    accountGoalDto.End = accountGoal.End;
                }
                return accountGoalDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Goal", ex.InnerException);
            }
        }
    }
}