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
    }
}