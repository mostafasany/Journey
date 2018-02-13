using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.Exceptions;
using Journey.Models;
using Journey.Services.Buisness.Account.Dto;

namespace Journey.Services.Buisness.Account.Translators
{
    public static class AccountDataTranslator
    {
        #region Transaltors

        public static AzureAccount TranslateAccount(Models.Account.Account account)
        {
            try
            {
                var accountDto = new AzureAccount();
                if (account == null) return accountDto;
                if (!string.IsNullOrEmpty(account.Id))
                    accountDto.Id = account.Id;
                accountDto.FName = account.FirstName;
                accountDto.LName = account.LastName;
                accountDto.Profile = account.Image?.Path;
                accountDto.SToken = account.SocialToken;
                accountDto.SProvider = account.SocialProvider;
                accountDto.SID = account.SID;
                accountDto.Email = account.Email;
                accountDto.Gender = account.Gender;
                accountDto.Status = account.Status;
                accountDto.Challenge = account.ChallengeId;
                return accountDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Account", ex.InnerException);
            }
        }

        public static List<Models.Account.Account> TranslateAccounts(List<AzureAccount> accounts)
        {
            try
            {
                return accounts.Select(TranslateAccount).ToList();
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Account", ex.InnerException);
            }
        }

        public static Models.Account.Account TranslateAccount(AzureAccount account)
        {
            try
            {
                var accountDto = new Models.Account.Account();
                if (account == null) return accountDto;
                if (!string.IsNullOrEmpty(account.Id))
                    accountDto.Id = account.Id;
                accountDto.FirstName = account.FName;
                accountDto.LastName = account.LName;
                accountDto.Image = new Media {Path = account.Profile};
                accountDto.SocialToken = account.SToken;
                accountDto.SocialProvider = account.SProvider;
                accountDto.SID = account.SID;
                accountDto.Email = account.Email;
                accountDto.Gender = account.Gender;
                accountDto.Status = account.Status;
                accountDto.ChallengeId = account.Challenge;
                return accountDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Account", ex.InnerException);
            }
        }

        #endregion
    }
}