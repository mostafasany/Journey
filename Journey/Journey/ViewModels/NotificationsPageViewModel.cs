using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Abstractions.Services;
using Abstractions.Services.Contracts;
using Journey.Models;
using Journey.Models.Account;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Friend;
using Journey.Services.Buisness.Notification;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class NotificationsPageViewModel : MainNavigationViewModel, INavigationAware
    {
        private readonly IDeepLinkService _deepLinking;
        private readonly INotificationService _notificationService;
        private readonly IFriendService _friendService;

        public NotificationsPageViewModel(IUnityContainer container,
                                          IAccountService accountService,
                                          INotificationService notificationService,
                                          IFriendService friendService,
            IDeepLinkService deepLinking) :
            base(container, accountService, notificationService)
        {
            _notificationService = notificationService;
            _deepLinking = deepLinking;
            _friendService = friendService;
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

        private ObservableCollection<FriendShip> _friendsRequestList;

        public ObservableCollection<FriendShip> FriendsRequestList
        {
            get => _friendsRequestList;
            set => SetProperty(ref _friendsRequestList, value);
        }

        private bool _isPullRefreshLoading;

        public bool IsPullRefreshLoading
        {
            get => _isPullRefreshLoading;
            set => SetProperty(ref _isPullRefreshLoading, value);
        }

        public bool NoNofications => (Notifications == null || Notifications.Count == 0) && (FriendsRequestList == null || FriendsRequestList.Count == 0);

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

                var friendRequests = await _friendService.GetFriendsRequestsAsync();
                if (friendRequests != null)
                    FriendsRequestList = new ObservableCollection<FriendShip>(friendRequests);

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
                _deepLinking.ParseDeepLinkingAndExecute(notification?.DeepLink);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnSelectedFriendCommand

        public DelegateCommand<FriendShip> OnSelectedFriendCommand => new DelegateCommand<FriendShip>(OnSelectedFriend);

        private void OnSelectedFriend(FriendShip selectedFriend)
        {
            try
            {
                if (selectedFriend == null || string.IsNullOrEmpty(selectedFriend.Id))
                    return;

                if (selectedFriend.FriendShipEnum == FriendShipEnum.Requested)
                    RequestApprove(selectedFriend);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }


        private async void RequestApprove(FriendShip selectedFriend)
        {
            var competeCommand = new DialogCommand
            {
                Label = AppResource.Yes,
                Invoked = async () =>
                {
                    bool status = await _friendService.FollowApproveAsync(selectedFriend.FriendShipId);
                    if (status)
                        FriendsRequestList.Remove(selectedFriend);
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

            string message = string.Format(AppResource.Friends_ApproveFriend, selectedFriend.Name);
            await DialogService.ShowMessageAsync("", message, commands);
        }

        #endregion

        #endregion
    }
}