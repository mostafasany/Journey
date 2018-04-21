using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Services.Azure;
using Journey.Services.Buisness.Account.Dto;
using Journey.Services.Buisness.Account.Translators;
using Journey.Services.Buisness.Friend.Dto;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Friend.Data
{
    public class FriendDataService : IFriendDataService
    {
        private readonly MobileServiceClient _client;
        private readonly IMobileServiceTable<AzureFriends> _accountFriendsTable;
        private readonly IMobileServiceTable<AzureAccount> _accountTable;
        //private const string ApiFriends = "https://graph.facebook.com/me/friends?fields=id,name,picture.type(large)&limit=999";

        public FriendDataService(IAzureService azureService)
        {
            _client = azureService.CreateOrGetAzureClient();
            _accountTable = _client.GetTable<AzureAccount>();
            _accountFriendsTable = _client.GetTable<AzureFriends>();
        }

        public async Task<bool> FollowAsync(string friend)
        {
            try
            {
                var api = "friends";
                var param = new Dictionary<string, string>();
                param.Add("action", friend + "," + "follow");
                bool success = await _client.InvokeApiAsync<bool>(api, HttpMethod.Put, param);
                return success;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<bool> UnFollowAsync(string friend)
        {
            try
            {
                var api = "friends";
                var param = new Dictionary<string, string>();
                param.Add("action", friend + "," + "unfollow");
                bool success = await _client.InvokeApiAsync<bool>(api, HttpMethod.Put, param);
                return success;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }


        public async Task<List<Models.Account.Account>> GetFriendsForChallengeAsync(string name)
        {
            try
            {
                string api = string.Format("Friends?action={0}", "friends");
                //var api = "friends";
                List<AzureAccount> accountTbl = await _client.InvokeApiAsync<List<AzureAccount>>(api, HttpMethod.Get, null);
                if (accountTbl != null)
                {
                    List<Models.Account.Account> accountDto = AccountDataTranslator.TranslateAccounts(accountTbl);
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