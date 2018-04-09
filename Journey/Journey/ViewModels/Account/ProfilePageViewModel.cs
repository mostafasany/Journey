using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Journey.Models.Account;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Notification;
using Prism.Commands;
using Unity;

namespace Journey.ViewModels
{
    public class ProfilePageViewModel : MainNavigationViewModel
    {
        private readonly IAccountService _accountService;

        public ProfilePageViewModel(IUnityContainer container, IAccountService accountService, INotificationService notificationService) :
            base(container, accountService, notificationService) => _accountService = accountService;

        #region Properties

        private Account _loggedInAccount;

        public Account LoggedInAccount
        {
            get => _loggedInAccount;
            set => SetProperty(ref _loggedInAccount, value);
        }


        private Account _friend;

        public Account Friend
        {
            get => _friend;
            set => SetProperty(ref _friend, value);
        }

        private string _firstTabSelected = "#ffffff";

        public string FirstTabSelected
        {
            get => _firstTabSelected;
            set => SetProperty(ref _firstTabSelected, value);
        }


        private string _secondTabSelected = "#ffffff";

        public string SecondTabSelected
        {
            get => _secondTabSelected;
            set => SetProperty(ref _secondTabSelected, value);
        }

        private string _thirdTabSelected = "#ffffff";

        public string ThirdTabSelected
        {
            get => _thirdTabSelected;
            set => SetProperty(ref _thirdTabSelected, value);
        }

        private string _fourthTabSelected = "#ffffff";

        public string FourthTabSelected
        {
            get => _fourthTabSelected;
            set => SetProperty(ref _fourthTabSelected, value);
        }

        private string _fifthTabSelected = "#ffffff";

        public string FifthTabSelected
        {
            get => _fifthTabSelected;
            set => SetProperty(ref _fifthTabSelected, value);
        }

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                await LoadAccount(false);

                base.Intialize(sync);
            }
            catch (Exception e)
            {
                ExceptionService.HandleAndShowDialog(e);
            }
        }

        private async Task LoadAccount(bool sync)
        {
            try
            {
                Account account = await _accountService.GetAccountAsync(sync);
                if (account != null)
                    LoggedInAccount = account;
                else
                    await DialogService.ShowMessageAsync(AppResource.Error, AppResource.Account_ErrorGetData);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
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

        protected  void ClearTabSelection()
        {
            try
            {
                FirstTabSelected = "#ffffff";
                SecondTabSelected = "#ffffff";
                ThirdTabSelected = "#ffffff";
                FourthTabSelected = "#ffffff";
                FifthTabSelected = "#ffffff";
            }
            catch (Exception e)
            {
                ExceptionService.HandleAndShowDialog(e);
            }
        }

        #endregion

        #region Commands

        #region OnLogoutCommand

        public DelegateCommand OnLogoutCommand => new DelegateCommand(OnLogout);

        private async void OnLogout()
        {
            try
            {
                var logoutCommand = new DialogCommand
                {
                    Label = AppResource.Yes,
                    Invoked = async () =>
                    {
                        await _accountService.LogoutAsync();
                        await NavigationService.Navigate("LoginPage", false);
                    }
                };

                var cancelCommand = new DialogCommand
                {
                    Label = AppResource.Cancel
                };
                var commands = new List<DialogCommand>
                {
                    logoutCommand,
                    cancelCommand
                };
                await DialogService.ShowMessageAsync("", AppResource.Logout, commands);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnEditProfileCommand

        public DelegateCommand OnEditProfileCommand => new DelegateCommand(OnEditProfile);

        private void OnEditProfile()
        {
            if (NavigationService.CurrentPage == "UpdateProfilePage")
                return;
            var parameters = new Dictionary<string, object>
            {
                {"Account", LoggedInAccount},
                {"ComeFromProfile", true}
            };
            NavigationService.Navigate("UpdateProfilePage", parameters);
        }

        #endregion

        #region OnLogWorkoutCommand

        public DelegateCommand OnLogWorkoutCommand => new DelegateCommand(OnLogWorkout);

        private void OnLogWorkout()
        {
            if (NavigationService.CurrentPage == "ProfileLogWorkoutPage")
                return;

            NavigationService.Navigate("ProfileLogWorkoutPage", null, null, null, false, true);
        }

        #endregion

        #region OnGoToProfileMeasurmentCommand

        public DelegateCommand OnGoToProfileMeasurmentCommand => new DelegateCommand(OnGoToProfileMeasurment);

        private void OnGoToProfileMeasurment()
        {
            if (NavigationService.CurrentPage == "ProfileMeasurmentPage")
                return;

            NavigationService.Navigate("ProfileMeasurmentPage", null, null, null, false, true);
        }

        #endregion

        #region OnGoToProfileChallengeCommand

        public DelegateCommand OnGoToProfileChallengeCommand => new DelegateCommand(OnGoToProfileChallenge);

        private void OnGoToProfileChallenge()
        {
            if (NavigationService.CurrentPage == "ProfileChallengePage")
                return;

            NavigationService.Navigate("ProfileChallengePage", null, null, null, false, true);
        }

        #endregion

        #region OnGoToActivityLogCommand

        public DelegateCommand OnGoToActivityLogCommand => new DelegateCommand(OnGoToActivityLog);

        private void OnGoToActivityLog()
        {
            if (NavigationService.CurrentPage == "ProfileActivityLogPage")
                return;

            NavigationService.Navigate("ProfileActivityLogPage", null, null, null, false, true);
        }

        #endregion

        #endregion
    }
}