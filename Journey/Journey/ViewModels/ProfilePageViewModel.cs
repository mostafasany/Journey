using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Journey.Models.Account;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Prism.Commands;
using Unity;
using Xamarin.Forms;

namespace Journey.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;

        public ProfilePageViewModel(IUnityContainer container, IAccountService accountService) :
            base(container)
        {
            _accountService = accountService;
        }

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

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();

                await LoadAccount(false);

                base.Intialize(sync);
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

        private async Task LoadAccount(bool sync)
        {
            try
            {
                var account = await _accountService.GetAccountAsync(sync);
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

        #endregion

        #region Commands

        #region OnMoreCommand

        public DelegateCommand OnMoreCommand => new DelegateCommand(OnMore);

        private async void OnMore()
        {
            try
            {
                var commands =
                    new List<DialogCommand>
                    {
                        new DialogCommand
                        {
                            Label = AppResource.Profile_Edit,
                            Invoked = () => OnEditProfile()
                        },

                        new DialogCommand
                        {
                            Label = AppResource.Logout,
                            Invoked = () => { OnLogoutCommand.Execute(); }
                        }
                    };

                await DialogService.ShowMessageAsync("", AppResource.More,
                    commands);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

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
            var parameters = new Dictionary<string, object>
            {
                {"Account", LoggedInAccount},
                {"ComeFromProfile", true}
            };
            NavigationService.Navigate("UpdateProfilePage", parameters);
        }

        #endregion

        #region OnGoToProfileMeasurmentCommand

        public DelegateCommand OnGoToProfileMeasurmentCommand => new DelegateCommand(OnGoToProfileMeasurment);

        private void OnGoToProfileMeasurment()
        {
            if (NavigationService.CurrentPage != "ProfileMeasurmentPage")
                NavigationService.Navigate("ProfileMeasurmentPage");
            var page = Application.Current.MainPage.Navigation;
        }

        #endregion

        #region OnGoToProfileChallengeCommand

        public DelegateCommand OnGoToProfileChallengeCommand => new DelegateCommand(OnGoToProfileChallenge);

        private void OnGoToProfileChallenge()
        {
            if (NavigationService.CurrentPage != "ProfileChallengePage")
                NavigationService.Navigate("ProfileChallengePage");
        }

        #endregion

        #region OnBackCommand

        public DelegateCommand OnBackCommand => new DelegateCommand(OnBack);


        private async void OnBack()
        {
            try
            {
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
            finally
            {
                HideProgress();
            }
        }

        #endregion

        #endregion
    }
}