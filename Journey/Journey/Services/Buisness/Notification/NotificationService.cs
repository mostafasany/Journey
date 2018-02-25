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
                var accountGoal = await _accountDataService.AddNotificationAsync(notification);
                return accountGoal;
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
                var accountGoal = await _accountDataService.GetNotificationsAsync();
                return accountGoal;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}