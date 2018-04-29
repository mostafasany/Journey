using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Challenge;
using Journey.Services.Buisness.Friend;
using Journey.Services.Buisness.Notification.Data;

namespace Journey.Services.Buisness.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationDataService _notificationDataService;
        private readonly IChallengeService _challengeService;
        private readonly IFriendService _friendService;
        private readonly IAccountService _accountService;
        public NotificationService(INotificationDataService notificationDataService,
                                   IChallengeService challengeService,
                                   IAccountService accountService,
                                   IFriendService friendService)
        {
            _notificationDataService = notificationDataService;
            _challengeService = challengeService;
            _friendService = friendService;
            _accountService = accountService;
        }

        public async Task<Notifications> AddNotificationAsync(Notifications notification)
        {
            try
            {
                Notifications notfication = await _notificationDataService.AddNotificationAsync(notification);
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
                List<Notifications> notfications = await _notificationDataService.GetNotificationsAsync();
                var challengeRequests = await _challengeService.GetChallengeRequestsAsync();
                if (challengeRequests != null)
                {
                    if (notfications == null)
                        notfications = new List<Notifications>();
                    foreach (var item in challengeRequests)
                    {
                        var challenger1 = item.ChallengeAccounts.FirstOrDefault();
                        var challenger2 = item.ChallengeAccounts.LastOrDefault();
                        Models.Account.Account tochallenge = null;
                        if (challenger1.Id == _accountService.LoggedInAccount.Id)
                        {
                            tochallenge = challenger2;
                        }
                        else
                        {
                            tochallenge = challenger1;
                        }
                        notfications.Add(new Notifications
                        {
                            Account = tochallenge,
                            Id = item.Id,
                            Title = AppResource.Notification_ChallengeRequest,
                            Message = AppResource.Notification_From + tochallenge.Name,
                            NotificationType = NotificationType.ChallengeRequest,
                        });
                    }

                }
                var friendRequests = await _friendService.GetFriendsRequestsAsync();
                if (friendRequests != null)
                {
                    if (notfications == null)
                        notfications = new List<Notifications>();
                    foreach (var item in friendRequests)
                    {
                        notfications.Add(new Notifications
                        {
                            Account = item,
                            Id = item.FriendShipId,
                            Title = AppResource.Notification_FriendRequest,
                            Message = AppResource.Notification_From + item.Name,
                            NotificationType = NotificationType.FriendRequest,
                        });
                    }
                }
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
                int notfications = await _notificationDataService.GetNotificationsCountAsync();
                return notfications;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}