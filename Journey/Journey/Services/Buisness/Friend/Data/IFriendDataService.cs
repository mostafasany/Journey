using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Account;

namespace Journey.Services.Buisness.Friend.Data
{
    public interface IFriendDataService
    {
        Task<bool> FollowRequestAsync(string friend);
        Task<bool> FollowRejectAsync(string frinedShipId);
        Task<bool> FollowApproveAsync(string frinedShipId);
        Task<List<Journey.Models.Account.Account>> GetFriendsForChallengeAsync(string name);
    }
}