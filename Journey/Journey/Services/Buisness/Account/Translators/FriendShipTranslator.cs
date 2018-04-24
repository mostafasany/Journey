using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.Exceptions;
using Abstractions.Forms;
using Journey.Models.Account;
using Journey.Services.Buisness.Friend.Dto;

namespace Journey.Services.Buisness.Account.Translators
{
    public static class FriendShipTranslator
    {
        public static FriendShip TranslateAccount(AzureFriendShip account)
        {
            try
            {
                var accountDto = new FriendShip();
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
                accountDto.FriendShipId = account.FriendShipId;
                accountDto.FriendShipStatus = account.FriendShipStatus;
                return accountDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("FriendShip", ex.InnerException);
            }
        }

        public static List<FriendShip> TranslateAccounts(List<AzureFriendShip> accounts)
        {
            try
            {
                return accounts.Select(TranslateAccount).ToList();
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("FriendShip", ex.InnerException);
            }
        }
    }
}