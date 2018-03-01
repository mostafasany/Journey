using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models;

namespace Journey.Services.Buisness.Notification.Data
{
    public interface INotificationDataService
    {
        Task<Notifications> AddNotificationAsync(Notifications notification);
        Task<List<Notifications>> GetNotificationsAsync();
    }
}