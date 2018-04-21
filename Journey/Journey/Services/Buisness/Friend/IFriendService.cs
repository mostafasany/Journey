using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journey.Services.Buisness.Friend
{
    public interface IFriendService
    {
        Task<bool> FollowAsync(string followerId);
        Task<bool> UnFollowAsync(string friend);
    }
}