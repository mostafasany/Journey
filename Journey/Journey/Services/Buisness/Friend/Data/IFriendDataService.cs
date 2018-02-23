using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journey.Services.Buisness.Friend.Data
{
    public interface IFriendDataService
    {
        Task<List<Models.Account.Account>> GetFriendsAsync(string name);

        Task<Models.Account.Account> GetFriendAsync(string id);

        Task<List<string>> FollowAsync(List<string> followerId);

        Task<bool> UnFollowAsync(string friendshipId);
    }
}