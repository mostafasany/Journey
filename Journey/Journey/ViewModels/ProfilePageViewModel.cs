using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Journey.Models.Account;
using Journey.Models.Post;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;

        public ProfilePageViewModel(IUnityContainer container, IAccountService accountService) :
            base(container)
        {
            _accountService = accountService;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                //if (parameters.GetNavigationMode() == NavigationMode.New)
                //    MediaList = parameters.GetValue<IEnumerable<Media>>("Media") ?? null;
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

        private Account loggedInAccount;

        public Account LoggedInAccount
        {
            get => loggedInAccount;
            set => SetProperty(ref loggedInAccount, value);
        }

        private bool isPullRefreshLoading;

        public bool IsPullRefreshLoading
        {
            get => isPullRefreshLoading;
            set => SetProperty(ref isPullRefreshLoading, value);
        }

        private List<ScaleMeasurment> measuremnts;

        public List<ScaleMeasurment> Measuremnts
        {
            get => measuremnts;
            set => SetProperty(ref measuremnts, value);
        }


        private Account friend;

        public Account Friend
        {
            get => friend;
            set => SetProperty(ref friend, value);
        }


        private double goal;

        public double Goal
        {
            get => goal;
            set => SetProperty(ref goal, value);
        }

        private DateTime end = DateTime.Now;

        public DateTime End
        {
            get => end;
            set => SetProperty(ref end, value);
        }

        private DateTime start = DateTime.Now;

        public DateTime Start
        {
            get => start;
            set => SetProperty(ref start, value);
        }


        private bool addMode;

        public bool AddMode
        {
            get => addMode;
            set => SetProperty(ref addMode, value);
        }

        #endregion

        #region Methods

        public override async void Intialize()
        {
            try
            {
                ShowProgress();
                IsPullRefreshLoading = false;
                await LoadAccount(false);
                base.Intialize();
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

        #region OnLogoutCommand

        public DelegateCommand OnLogoutCommand => new DelegateCommand(OnLogout);

        private async void OnLogout()
        {
            try
            {
                var logoutCommand = new DialogCommand
                {
                    Label = AppResource.Logout,
                    Invoked = async () =>
                    {
                        await _accountService.LogoutAsync();
                        NavigationService.Navigate("LoginPage", false);
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
            NavigationService.Navigate("UpdateProfilePage");
        }

        #endregion

        #region OnAddGoalCommand

        public DelegateCommand OnAddGoalCommand => new DelegateCommand(OnAddGoal);

        private async void OnAddGoal()
        {
            try
            {
                AddMode = false;

                LoggedInAccount.AccountGoal.Goal = Goal;
                LoggedInAccount.AccountGoal.Start = Start;
                LoggedInAccount.AccountGoal.End = End;
                //LoggedInAccount.AccountGoal = await accountGoalService.AddAccountGoal(LoggedInAccount.AccountGoal);
                Goal = 0;
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