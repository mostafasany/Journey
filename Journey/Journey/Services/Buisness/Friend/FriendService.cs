using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Services.Buisness.Friend.Data;

namespace Journey.Services.Buisness.Friend
{
    public class FriendService : IFriendService
    {
        private readonly IFriendDataService _friendDataService;

        public FriendService(IFriendDataService friendDataService) => _friendDataService = friendDataService;

        public async Task<List<string>> FollowAsync(List<string> followerId)
        {
            try
            {
                List<string> failureIds = await _friendDataService.FollowAsync(followerId);
                return failureIds;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<bool> UnFollowAsync(string friendshipId)
        {
            try
            {
                bool status = await _friendDataService.UnFollowAsync(friendshipId);
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

        public async Task<List<Models.Account.Account>> GetFriendsAsync(string name)
        {
            try
            {
                List<Models.Account.Account> friends = await _friendDataService.GetFriendsAsync(name);
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
                //var gallery = await GetAccountGalleryAsync();
                List<Models.Account.Account> inpiredList = await GetFriendsAsync("");
                //inpiredList.Select(a => a.Status = "Achieve and celebrate your health goals").ToList();
                //inpiredList.Select(a => a.MediaList = gallery).ToList();

                return inpiredList;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}