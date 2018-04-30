using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.Exceptions;
using Abstractions.Forms;
using Journey.Models.Account;
using Journey.Services.Buisness.Friend.Dto;

namespace Journey.Services.Buisness.Friend.Translators
{
    public static class FriendShipTranslator
    {
        public static FriendShip TranslateAccount(AzureFriendShip friend)
        {
            try
            {
                var friendShip = new FriendShip();
                if (friend == null) return friendShip;
                if (!string.IsNullOrEmpty(friend.Id))
                    friendShip.Id = friend.Id;
                friendShip.FirstName = friend.FName;
                friendShip.LastName = friend.LName;
                friendShip.Image = new Media {Path = friend.Profile};
                friendShip.SocialToken = friend.SToken;
                friendShip.SocialProvider = friend.SProvider;
                friendShip.SID = friend.SID;
                friendShip.Email = friend.Email;
                friendShip.Gender = friend.Gender;
                friendShip.Status = friend.Status;
                friendShip.ChallengeId = friend.Challenge;
                friendShip.FriendShipId = friend.FriendShipId;
                friendShip.FriendShipStatus = friend.FriendShipStatus;
                return friendShip;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("FriendShip", ex.InnerException);
            }
        }

        public static List<FriendShip> TranslateAccounts(List<AzureFriendShip> friends)
        {
            try
            {
                return friends == null ? new List<FriendShip>() : friends.Select(TranslateAccount).ToList();
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("FriendShip", ex.InnerException);
            }
        }
    }
}