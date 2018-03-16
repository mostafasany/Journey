using System;
using System.Collections.Generic;
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
        private readonly IMobileServiceTable<AzureFriends> accountFriendsTable;
        private readonly IMobileServiceTable<AzureAccount> accountTable;

        //private const string ApiFriends = "https://graph.facebook.com/me/friends?fields=id,name,picture.type(large)&limit=999";

        public FriendDataService(IAzureService azureService)
        {
            _client = azureService.CreateOrGetAzureClient();
            accountTable = _client.GetTable<AzureAccount>();
            accountFriendsTable = _client.GetTable<AzureFriends>();
        }


        public async Task<List<string>> FollowAsync(List<string> followerId)
        {
            try
            {
                var failureRequest = new List<string>();
                foreach (var friend in followerId)
                    try
                    {
                        var newFriend = new AzureFriends {Accoun1 = _client.CurrentUser.UserId, Account2 = friend};
                        await accountFriendsTable.InsertAsync(newFriend);
                        if (string.IsNullOrEmpty(newFriend.Id))
                            failureRequest.Add(friend);
                    }
                    catch (Exception)
                    {
                        failureRequest.Add(friend);
                    }
                return failureRequest;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<bool> UnFollowAsync(string friendshipId)
        {
            try
            {
                var deleteFriend = new AzureFriends {Id = friendshipId};
                await accountFriendsTable.DeleteAsync(deleteFriend);
                return true;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<Models.Account.Account> GetFriendAsync(string friend)
        {
            try
            {
                // string api = string.Format("friends?id={0}", friend);
                // List<AzureFriend> accountsTbl = await _client.InvokeApiAsync<List<AzureFriend>>(api, HttpMethod.Get, null);

                Models.Account.Account account = null;
                // accountTbl = await accountTable.Where(a => a.Id != account).ToListAsync();
                //if (accountsTbl != null && accountsTbl.Count != 0)
                //{
                //    AzureFriend friendDTO = accountsTbl.FirstOrDefault();
                //    account = TranslateAccount(friendDTO);
                //}
                //else
                //{
                //    //TODO: Query is not correct ,if not friend found it return null , it should return account with no friend
                var accountDTO = await accountTable.LookupAsync(friend);
                account = AccountDataTranslator.TranslateAccount(accountDTO);
                //}


                return account;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<List<Models.Account.Account>> GetFriendsAsync(string name)
        {
            try
            {
                List<AzureAccount> accountTbl = null;
                var account = _client.CurrentUser.UserId;
                if (!string.IsNullOrEmpty(name))
                    accountTbl = await accountTable.Where(a => a.Id != account &&
                                                               (a.FName.ToLower().Contains(name.ToLower()) || a.LName
                                                                    .ToLower().Contains(name.ToLower()))).ToListAsync();
                else
                    accountTbl = await accountTable.Where(a => a.Id != account).ToListAsync();
                var accountDto = AccountDataTranslator.TranslateAccounts(accountTbl);
                return accountDto;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }
    }
}