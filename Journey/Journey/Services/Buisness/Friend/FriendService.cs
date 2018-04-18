using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Account;
using Journey.Services.Buisness.Friend.Data;

namespace Journey.Services.Buisness.Friend
{
    public class FriendService : IFriendService
    {
        private readonly IFriendDataService _friendDataService;

        public FriendService(IFriendDataService friendDataService) => _friendDataService = friendDataService;

        public async Task<bool> FollowAsync(string followerId)
        {
            try
            {
                bool followed = await _friendDataService.FollowAsync(followerId);
                return followed;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<bool> UnFollowAsync(string friend)
        {
            try
            {
                bool status = await _friendDataService.UnFollowAsync(friend);
                return status;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<Models.Account.Account> GetFriendAsync(string id)
        {
            try
            {
                Models.Account.Account acc = await _friendDataService.GetFriendAsync(id);
                return acc;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<List<Models.Account.Account>> SearchForFriendsAsync(string name)
        {
            try
            {
                List<Models.Account.Account> friends = await _friendDataService.SearchForFriendsAsync(name);
                return friends;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<List<Models.Account.Account>> GetInspiredListAsync()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<List<Models.Account.Account>> GetFriendsForChallengeAsync(string name)
        {
            try
            {
                List<Models.Account.Account> friends = await _friendDataService.GetFriendsForChallengeAsync(name);
                return friends;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}