using System;
using Abstractions.Exceptions;
using Journey.Models.Account;
using Journey.Services.Buisness.Goal.Dto;

namespace Journey.Services.Buisness.Goal.Translators
{
    public static class AccountGoalDataTranslator
    {
        #region Transaltors

        public static AccountGoal TranslateAccountGoal(AzureAccountGoal accountGoal)
        {
            try
            {
                var accountDto = new AccountGoal();
                if (accountGoal != null)
                {
                    accountDto.Weight = accountGoal.Weight;
                    accountDto.Goal = accountGoal.Goal;
                    accountDto.Start = accountGoal.Start;
                    accountDto.End = accountGoal.End;
                }

                return accountDto;
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
                var accountDto = new AzureAccountGoal();
                if (accountGoal != null)
                {
                    accountDto.Account = account;
                    accountDto.Weight = accountGoal.Weight;
                    accountDto.Goal = accountGoal.Goal;
                    accountDto.Start = accountGoal.Start;
                    accountDto.End = accountGoal.End;
                }
                return accountDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Goal", ex.InnerException);
            }
        }

        #endregion
    }
}