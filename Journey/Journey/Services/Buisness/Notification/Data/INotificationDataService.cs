using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journey.Services.Buisness.Notification.Data
{
    public interface INotificationDataService
    {
        Task<Models.Notifications> AddNotificationAsync(Models.Notifications notification);
        Task<List<Models.Notifications>> GetNotificationsAsync();
    }
}