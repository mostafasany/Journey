using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.Exceptions;
using Journey.Models;
using Journey.Services.Buisness.Notification.Dto;

namespace Journey.Services.Buisness.Notification.Translators
{
    public static class NotificationDataTranslator
    {
        public static AzureNotifications TranslateNotification(Notifications account)
        {
            try
            {
                var accountDto = new AzureNotifications();
                if (account == null) return accountDto;

                accountDto.Id = account.Id;
                accountDto.Title = account.Title;
                accountDto.Message = account.Message;
                accountDto.DeepLink = account.DeepLink;
                accountDto.Account = account.Id;

                return accountDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Notification", ex.InnerException);
            }
        }

        public static List<Notifications> TranslateNotifications(List<AzureNotifications> notifications)
        {
            try
            {
                return notifications.Select(TranslateNotification).ToList();
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Notification", ex.InnerException);
            }
        }

        public static Notifications TranslateNotification(AzureNotifications account)
        {
            try
            {
                var accountDto = new Notifications
                {
                    Id = account.Id,
                    Title = account.Title,
                    Message = account.Message,
                    DeepLink = account.DeepLink
                };
                // accountDto.Account = account?.Account.;

                return accountDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Notification", ex.InnerException);
            }
        }
    }
}