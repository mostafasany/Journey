using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Abstractions.Services;
using Abstractions.Services.Contracts;
using Journey.Models;
using Journey.Models.Challenge;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Challenge;
using Journey.Services.Buisness.Friend;
using Journey.Services.Buisness.Notification;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class NotificationsPageViewModel : MainNavigationViewModel, INavigationAware
    {
        private readonly IChallengeService _challengeService;
        private readonly IDeepLinkService _deepLinking;
        private readonly IFriendService _friendService;
        private readonly INotificationService _notificationService;

        public NotificationsPageViewModel(IUnityContainer container,
            IAccountService accountService,
            INotificationService notificationService,
            IFriendService friendService,
            IChallengeService challengeService,
            IDeepLinkService deepLinking) :
            base(container, accountService, notificationService)
        {
            _notificationService = notificationService;
            _deepLinking = deepLinking;
            _friendService = friendService;
            _challengeService = challengeService;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                Intialize();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        #endregion

        #region Properties

        private ObservableCollection<Notifications> _notifications;

        public ObservableCollection<Notifications> Notifications
        {
            get => _notifications;
            set
            {
                _notifications = value;
                RaisePropertyChanged();
            }
        }

        private Notifications _selectedNotification;

        public Notifications SelectedNotification
        {
            get => _selectedNotification;
            set
            {
                SetProperty(ref _selectedNotification, value);

                if (value != null)
                    OnSelectedNotificationCommand.Execute(value);
            }
        }

        private bool _isPullRefreshLoading;

        public bool IsPullRefreshLoading
        {
            get => _isPullRefreshLoading;
            set => SetProperty(ref _isPullRefreshLoading, value);
        }

        public bool NoNofications => Notifications == null || Notifications.Count == 0;

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                base.Intialize(sync);

                Notifications = new ObservableCollection<Notifications>();
                List<Notifications> postDTo = await _notificationService.GetNotificationsAsync();
                if (postDTo != null)
                    Notifications = new ObservableCollection<Notifications>(postDTo);

                RaisePropertyChanged(nameof(NoNofications));
            }
            catch (Exception e)
            {
                ExceptionService.HandleAndShowDialog(e);
            }
            finally
            {
                HideProgress();
            }
        }

        protected override void Cleanup()
        {
            try
            {
                //Here Cleanup any resources
                base.Cleanup();
            }
            catch (Exception e)
            {
                ExceptionService.HandleAndShowDialog(e);
            }
        }

        private async void RequestFriendRequestApproval(Notifications notification)
        {
            var competeCommand = new DialogCommand
            {
                Label = AppResource.Yes,
                Invoked = async () =>
                {
                    bool status = await _friendService.FollowApproveAsync(notification.Id);
                    if (status)
                        Notifications.Remove(notification);
                }
            };

            var cancelCommand = new DialogCommand
            {
                Label = AppResource.Cancel
            };

            var commands = new List<DialogCommand>
            {
                competeCommand,
                cancelCommand
            };

            string message = string.Format(AppResource.Friends_ApproveFriend, notification.Account.Name);
            await DialogService.ShowMessageAsync("", message, commands);
        }

        private async void RequestChallengeRequestApproval(Notifications notification)
        {
            var competeCommand = new DialogCommand
            {
                Label = AppResource.Yes,
                Invoked = async () =>
                {
                    Challenge challenge = await _challengeService.ApproveChallengeAsync(notification.Id);
                    if (challenge != null)
                        Notifications.Remove(notification);
                }
            };

            var cancelCommand = new DialogCommand
            {
                Label = AppResource.Cancel
            };

            var commands = new List<DialogCommand>
            {
                competeCommand,
                cancelCommand
            };

            string message = string.Format(AppResource.Challenge_ApproveChallenge, notification.Account.Name);
            await DialogService.ShowMessageAsync("", message, commands);
        }

        #endregion

        #region Commands

        #region OnCloseCommand

        public DelegateCommand OnCloseCommand => new DelegateCommand(OnClose);

        private void OnClose()
        {
            NavigationService.GoBack();
        }

        #endregion

        #region OnPullRefreshRequestCommand

        public DelegateCommand OnPullRefreshRequestCommand => new DelegateCommand(OnPullRefreshRequest);

        private void OnPullRefreshRequest()
        {
            try
            {
                IsPullRefreshLoading = true;

                Intialize();
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
            finally
            {
                IsPullRefreshLoading = false;
                HideProgress();
            }
        }

        #endregion

        #region OnSelectedNotificationCommand

        public DelegateCommand<Notifications> OnSelectedNotificationCommand => new DelegateCommand<Notifications>(
            OnSelectedNotification);

        private void OnSelectedNotification(Notifications notification)
        {
            try
            {
                if (notification.NotificationType == NotificationType.Notification)
                    _deepLinking.ParseDeepLinkingAndExecute(notification.DeepLink);
                else if (notification.NotificationType == NotificationType.FriendRequest)
                    RequestFriendRequestApproval(notification);
                else if (notification.NotificationType == NotificationType.ChallengeRequest) RequestChallengeRequestApproval(notification);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #endregion
    }
}