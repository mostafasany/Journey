using System;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Services.Buisness.Friend.Data;

namespace Journey.Services.Buisness.Friend
{
    public class FriendService : IFriendService
    {
        private readonly IFriendDataService _friendDataService;

        public FriendService(IFriendDataService friendDataService) => _friendDataService = friendDataService;

        public async Task<bool> FollowRequestAsync(string friend)
        {
            try
            {
                bool followed = await _friendDataService.FollowRequestAsync(friend);
                return followed;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<bool> FollowApproveAsync(string frinedShipId)
        {
            try
            {
                bool status = await _friendDataService.FollowApproveAsync(frinedShipId);
                return status;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<bool> FollowRejectAsync(string frinedShipId)
        {
            try
            {
                bool status = await _friendDataService.FollowRejectAsync(frinedShipId);
                return status;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<bool> IgnoreApproveAsync(string frinedShipId)
        {
            try
            {
                bool status = await _friendDataService.IgnoreApproveAsync(frinedShipId);
                return status;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}