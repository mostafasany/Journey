using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Account;

namespace Tawasol.Services.Data
{
    public interface IFriendDataService
    {
        Task<List<Account>> GetFriendsAsync(string name);

        Task<Account> GetFriendAsync(string id);

        Task<List<string>> FollowAsync(List<string> followerId);

        Task<bool> UnFollowAsync(string friendshipId);
    }
}
