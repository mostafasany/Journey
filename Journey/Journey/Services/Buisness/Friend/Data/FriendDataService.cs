using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Account;
using Journey.Services.Azure;
using Journey.Services.Buisness.Account.Translators;
using Journey.Services.Buisness.Friend.Dto;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Friend.Data
{
    public class FriendDataService : IFriendDataService
    {
        private readonly MobileServiceClient _client;

        //private const string ApiFriends = "https://graph.facebook.com/me/friends?fields=id,name,picture.type(large)&limit=999";

        public FriendDataService(IAzureService azureService) => _client = azureService.CreateOrGetAzureClient();

        public async Task<bool> FollowRequestAsync(string friend) => await ChangeStatusAsync(friend, "request");

        public async Task<bool> FollowRejectAsync(string frinedShipId) => await ChangeStatusAsync(frinedShipId, "ignore");

        public async Task<bool> FollowApproveAsync(string frinedShipId) => await ChangeStatusAsync(frinedShipId, "approve");

        public async Task<bool> IgnoreApproveAsync(string frinedShipId) => await ChangeStatusAsync(frinedShipId, "ignore");

        public async Task<List<FriendShip>> GetFriendsForChallengeAsync(string keyword) => await CallFriendsApiAsync("friends", keyword);

        public async Task<List<FriendShip>> FindAccontAsync(string keyword) => await CallFriendsApiAsync("all", keyword);

        public async Task<List<FriendShip>> GetFriendsRequestsAsync() => await CallFriendsApiAsync("requests", "");

        public async Task<bool> ChangeStatusAsync(string frinedShipId, string status)
        {
            try
            {
                var api = "friends";
                var param = new Dictionary<string, string> { { "action", frinedShipId + "," + status } };
                bool success = await _client.InvokeApiAsync<bool>(api, HttpMethod.Put, param);
                return success;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        private async Task<List<FriendShip>> CallFriendsApiAsync(string action, string keyword)
        {
            try
            {
                string api = string.Format("Friends?action={0}", action);
                // var api = "friends";
                List<AzureFriendShip> accountTbl = await _client.InvokeApiAsync<List<AzureFriendShip>>(api, HttpMethod.Get, null);
                if (accountTbl != null)
                {
                    List<FriendShip> accountDto = FriendShipTranslator.TranslateAccounts(accountTbl);
                    return accountDto;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }
    }
}