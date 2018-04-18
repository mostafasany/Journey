using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journey.Services.Buisness.Friend
{
    public interface IFriendService
    {
        Task<bool> FollowAsync(string followerId);
        Task<Models.Account.Account> GetFriendAsync(string id);
        Task<List<Models.Account.Account>> SearchForFriendsAsync(string name);
        Task<List<Models.Account.Account>> GetFriendsForChallengeAsync(string name);
        Task<List<Models.Account.Account>> GetInspiredListAsync();
        Task<bool> UnFollowAsync(string friend);
    }
}