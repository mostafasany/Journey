using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journey.Services.Buisness.Friend.Data
{
    public interface IFriendDataService
    {
        Task<bool> UnFollowAsync(string friend);
        Task<bool> FollowAsync(string followerId);
        Task<Models.Account.Account> GetFriendAsync(string id);
        Task<List<Models.Account.Account>> SearchForFriendsAsync(string name);
        Task<List<Models.Account.Account>> GetFriendsForChallengeAsync(string name);
    }
}