using System;
using System.Windows.Input;
using Abstractions.Forms;
using Journey.Models.Account;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Notification;
using Prism.Commands;
using Unity;

namespace Journey.ViewModels
{
    public class MainNavigationViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly INotificationService _notificationService;

        private bool _hasActiveChallenge;

        private Account _loggedInAccount;

        private int _notificationsCount;

        public MainNavigationViewModel(IUnityContainer container, IAccountService accountService, INotificationService notificationService) : base(container)
        {
            _notificationService = notificationService;
            _accountService = accountService;
            LoadAccount();
            LoadNotificationsCount();
        }

        public bool HasActiveChallenge
        {
            get => _hasActiveChallenge;
            set => SetProperty(ref _hasActiveChallenge, value);
        }

        public Media Image =>
            LoggedInAccount == null
                ? new Media {Path = "https://bit.ly/2HezBsF"}
                : _loggedInAccount.Image;

        public Account LoggedInAccount
        {
            get => _loggedInAccount;
            set
            {
                SetProperty(ref _loggedInAccount, value);
                RaisePropertyChanged(nameof(Image));
            }
        }

        public int NotificationsCount
        {
            get => _notificationsCount;
            set => SetProperty(ref _notificationsCount, value);
        }

        private async void LoadAccount()
        {
            try
            {
                LoggedInAccount = await _accountService.GetAccountAsync();
                UpdateChallengeBanner();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleAndShowDialog(ex);
            }
        }

        private async void LoadNotificationsCount()
        {
            try
            {
                NotificationsCount = await _notificationService.GetNotificationsCountAsync();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleAndShowDialog(ex);
            }
        }

        private void UpdateChallengeBanner()
        {
            try
            {
                HasActiveChallenge = LoggedInAccount != null && !LoggedInAccount.HasNotActiveChallenge;
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #region OnProfileCommand

        private ICommand _onProfileCommand;

        public ICommand OnProfileCommand => _onProfileCommand ??
                                            (_onProfileCommand =
                                                new DelegateCommand(OnProfile));

        private async void OnProfile()
        {
            try
            {
                bool isLogginIn = await _accountService.LoginFirstAsync();
                if (isLogginIn)
                    if (NavigationService.CurrentPage != "ProfileActivityLogPage")
                        await NavigationService.Navigate("ProfileActivityLogPage");
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnHomeCommand

        private ICommand _onHomeCommand;

        public ICommand OnHomeCommand => _onHomeCommand ?? (_onHomeCommand = new DelegateCommand(OnHome));

        private async void OnHome()
        {
            try
            {
                if (NavigationService.CurrentPage != "HomePage")
                    await NavigationService.Navigate("HomePage");
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnChallengeCommand

        public DelegateCommand OnChallengeCommand => new DelegateCommand(OnChallenge);

        private async void OnChallenge()
        {
            if (HasActiveChallenge)
            {
                if (NavigationService.CurrentPage == "ChallengeProgressPage")
                    return;

                await NavigationService.Navigate("ChallengeProgressPage", null, null, null, false, true);
            }
            else
            {
                bool isLogginIn = await _accountService.LoginFirstAsync();
                if (isLogginIn && NavigationService.CurrentPage != "StartNewChallengePage")
                    await NavigationService.Navigate("StartNewChallengePage");
            }
        }

        #endregion

        #region OnSearchFriendCommand

        public DelegateCommand OnSearchFriendCommand => new DelegateCommand(OnSearchFriend);

        private async void OnSearchFriend()
        {
            bool isLogginIn = await _accountService.LoginFirstAsync();
            if (isLogginIn)
                if (NavigationService.CurrentPage != "FriendsPage")
                    await NavigationService.Navigate("FriendsPage");
        }

        #endregion

        #region OnNotificationCommand

        private ICommand _onNotificationCommand;

        public ICommand OnNotificationCommand => _onNotificationCommand ??
                                                 (_onNotificationCommand =
                                                     new DelegateCommand(OnNotification));

        private async void OnNotification()
        {
            try
            {
                bool isLogginIn = await _accountService.LoginFirstAsync();
                if (isLogginIn)
                    if (NavigationService.CurrentPage != "NotificationsPage")
                        await NavigationService.Navigate("NotificationsPage");
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion
    }
}