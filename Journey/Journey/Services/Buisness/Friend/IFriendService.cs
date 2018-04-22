using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journey.Services.Buisness.Friend
{
    public interface IFriendService
    {
        Task<bool> FollowRequestAsync(string friend);
        Task<bool> FollowRejectAsync(string frinedShipId);
        Task<bool> FollowApproveAsync(string frinedShipId);
    }
}