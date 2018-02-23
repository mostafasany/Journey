using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journey.Services.Buisness.Friend
{
    public interface IFriendService
    {
        Task<List<Models.Account.Account>> GetFriendsAsync(string name);

        Task<Models.Account.Account> GetFriendAsync(string id);

        Task<List<string>> FollowAsync(List<string> followerId);

        Task<bool> UnFollowAsync(string friendshipId);

        Task<List<Models.Account.Account>> GetInspiredListAsync();
    }
}