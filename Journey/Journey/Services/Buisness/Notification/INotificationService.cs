using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Account;

namespace Journey.Services.Buisness.Notification
{
    public interface INotificationService
    {
        Task<Models.Notifications> AddNotificationAsync(Models.Notifications notification);
        Task<List<Models.Notifications>> GetNotificationsAsync();
    }
}