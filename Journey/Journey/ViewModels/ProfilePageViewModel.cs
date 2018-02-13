using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Journey.Models.Account;
using Journey.Models.Post;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Goal;
using Journey.Services.Buisness.Measurment;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountGoalService _accountGoalService;
        private readonly IAccountMeasurmentService _accountMeasurmentService;
        private readonly IAccountService _accountService;

        public ProfilePageViewModel(IUnityContainer container, IAccountService accountService,
            IAccountGoalService accountGoalService, IAccountMeasurmentService accountMeasurmentService) :
            base(container)
        {
            _accountService = accountService;
            _accountGoalService = accountGoalService;
            _accountMeasurmentService = accountMeasurmentService;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public  void OnNavigatedTo(NavigationParameters parameters)
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

        private Account _loggedInAccount;

        public Account LoggedInAccount
        {
            get => _loggedInAccount;
            set => SetProperty(ref _loggedInAccount, value);
        }

        private bool _isPullRefreshLoading;

        public bool IsPullRefreshLoading
        {
            get => _isPullRefreshLoading;
            set => SetProperty(ref _isPullRefreshLoading, value);
        }

        private List<ScaleMeasurment> _measuremnts;

        public List<ScaleMeasurment> Measuremnts
        {
            get => _measuremnts;
            set => SetProperty(ref _measuremnts, value);
        }


        private Account _friend;

        public Account Friend
        {
            get => _friend;
            set => SetProperty(ref _friend, value);
        }


        private double _goal;

        public double Goal
        {
            get => _goal;
            set => SetProperty(ref _goal, value);
        }

        private DateTime _end = DateTime.Now;

        public DateTime End
        {
            get => _end;
            set => SetProperty(ref _end, value);
        }

        private DateTime _start = DateTime.Now;

        public DateTime Start
        {
            get => _start;
            set => SetProperty(ref _start, value);
        }


        private bool _addMode;

        public bool AddMode
        {
            get => _addMode;
            set => SetProperty(ref _addMode, value);
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
                await LoadGoal(false);
                await LoadMeasurments(false);
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

        private async Task LoadGoal(bool sync)
        {
            try
            {
                var accountGoal = await _accountGoalService.GetAccountGoalAsync(sync);
                if (accountGoal != null)
                    LoggedInAccount.AccountGoal = accountGoal;
                else
                    await DialogService.ShowMessageAsync(AppResource.Error, AppResource.Account_ErrorGetData);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        private async Task LoadMeasurments(bool sync)
        {
            try
            {
                if (Measuremnts == null || Measuremnts.Count == 0 || sync)
                {
                    var measu = await _accountMeasurmentService.GetMeasurmentsAsync(sync);
                    if (measu != null)
                    {
                        Measuremnts = measu;
                    }
                    else
                    {
                        await DialogService.ShowMessageAsync(AppResource.Error, AppResource.Account_ErrorGetData);
                        return;
                    }
                }

                if (LoggedInAccount.AccountGoal == null)
                    LoggedInAccount.AccountGoal = new AccountGoal();
                if (LoggedInAccount.AccountGoal?.Weight == null || LoggedInAccount.AccountGoal.Weight == 0)
                    LoggedInAccount.AccountGoal.Weight = Measuremnts.FirstOrDefault(a => a.Title == "Weight").Measure;
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
            NavigationService.Navigate("UpdateProfilePage", LoggedInAccount, "Account");
        }

        #endregion

        #region OnAddGoalCommand

        public DelegateCommand OnAddGoalCommand => new DelegateCommand(OnAddGoal);

        private  void OnAddGoal()
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
                            Label = AppResource.Profile_SetMonthlyGoal,
                            Invoked = () => { AddMode = true; }
                        },
                        new DialogCommand
                        {
                            Label = AppResource.Profile_UpdateMeasurment,
                            Invoked = () => { NavigationService.Navigate("UpdateMeasurmentPage"); }
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

        #endregion
    }
}