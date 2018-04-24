using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Account;

namespace Journey.Services.Buisness.Friend
{
    public interface IFriendService
    {
        Task<List<FriendShip>> FindAccontAsync(string keyword);
        Task<bool> FollowApproveAsync(string frinedShipId);
        Task<bool> FollowRejectAsync(string frinedShipId);
        Task<bool> FollowRequestAsync(string friend);
        Task<List<FriendShip>> GetFriendsForChallengeAsync(string keyword);
        Task<List<FriendShip>> GetFriendsRequestsAsync();
        Task<bool> IgnoreApproveAsync(string frinedShipId);
    }
}