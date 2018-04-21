using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models;
using Journey.Services.Azure;
using Journey.Services.Buisness.Notification.Dto;
using Journey.Services.Buisness.Notification.Translators;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Notification.Data
{
    public class NotificationDataService : INotificationDataService
    {
        private readonly IMobileServiceTable<AzureNotifications> _azureNotifications;
        private readonly MobileServiceClient _client;

        public NotificationDataService(IAzureService azureService)
        {
            _client = azureService.CreateOrGetAzureClient();
            _azureNotifications = _client.GetTable<AzureNotifications>();
        }

        public async Task<Notifications> AddNotificationAsync(Notifications notification)
        {
            try
            {
                if (notification == null)
                    return null;
                AzureNotifications accountDto = NotificationDataTranslator.TranslateNotification(notification);
                await _azureNotifications.InsertAsync(accountDto);
                notification = NotificationDataTranslator.TranslateNotification(accountDto);
                return notification;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<List<Notifications>> GetNotificationsAsync()
        {
            try
            {
                string account = _client.CurrentUser.UserId;
                List<AzureNotifications> notifications = await _azureNotifications.Where(po => po.Account == account).ToListAsync();
                if (notifications == null || notifications.Count == 0)
                    return null;

                List<Notifications> commentsDTo = NotificationDataTranslator.TranslateNotifications(notifications);
                return commentsDTo;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<int> GetNotificationsCountAsync()
        {
            try
            {
                string account = _client.CurrentUser.UserId;
                List<AzureNotifications> notifications = await _azureNotifications.Where(po => po.Account == account).ToListAsync();
                if (notifications == null || notifications.Count == 0)
                    return 0;
                return notifications.Count;
            }
            catch (HttpRequestException ex)
            {
                throw new NoInternetException(ex);
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }
    }
}