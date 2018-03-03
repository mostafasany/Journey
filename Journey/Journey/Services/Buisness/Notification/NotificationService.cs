using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models;
using Journey.Services.Buisness.Notification.Data;

namespace Journey.Services.Buisness.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationDataService _accountDataService;

        public NotificationService(INotificationDataService accountDataService)
        {
            _accountDataService = accountDataService;
        }

        public async Task<Notifications> AddNotificationAsync(Notifications notification)
        {
            try
            {
                var notfication = await _accountDataService.AddNotificationAsync(notification);
                return notfication;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<List<Notifications>> GetNotificationsAsync()
        {
            try
            {
                var notfications = await _accountDataService.GetNotificationsAsync();
                return notfications;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<int> GetNotificationsCountAsync()
        {
            try
            {
                var notfications = await _accountDataService.GetNotificationsCountAsync();
                return notfications;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}