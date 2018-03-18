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
using Journey.Services.Buisness.Notification;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ProfileMeasurmentPageViewModel : ProfilePageViewModel, INavigationAware
    {
        private readonly IAccountGoalService _accountGoalService;
        private readonly IAccountMeasurmentService _accountMeasurmentService;

        public ProfileMeasurmentPageViewModel(IUnityContainer container, IAccountService accountService,INotificationService notificationService,
            IAccountGoalService accountGoalService, IAccountMeasurmentService accountMeasurmentService) :
        base(container, accountService,notificationService)
        {
            _accountGoalService = accountGoalService;
            _accountMeasurmentService = accountMeasurmentService;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                SecondTabSelected = "#ffffff";
                FirstTabSelected = "#ffffff";
                ThirdTabSelected = "#f1f1f1";
                FourthTabSelected = "#ffffff";

                if (parameters.GetNavigationMode() == NavigationMode.Back)
                {
                    var measurments = parameters.GetValue<List<ScaleMeasurment>>("Measurments");
                    if (measurments != null)
                    {
                        Measuremnts = measurments;
                        LoggedInAccount.AccountGoal.Weight = Measuremnts.FirstOrDefault().Measure;
                        LoggedInAccount.AccountGoal =
                            await _accountGoalService.AddAccountGoal(LoggedInAccount.AccountGoal);
                    }
                }

                if (parameters.GetNavigationMode() == NavigationMode.New)
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
            set
            {
                SetProperty(ref _addMode, value);
                RaisePropertyChanged(nameof(NotAddMode));
            }
        }

        public bool NotAddMode => !_addMode;


        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                IsPullRefreshLoading = false;
                base.Intialize(sync);
                await LoadGoal(false);
                await LoadMeasurments(false);
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

        private async Task LoadGoal(bool sync)
        {
            try
            {
                AccountGoal accountGoal = await _accountGoalService.GetAccountGoalAsync(sync);
                if (accountGoal != null)
                    LoggedInAccount.AccountGoal = accountGoal;
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
                    List<ScaleMeasurment> measu = await _accountMeasurmentService.GetMeasurmentsAsync(sync);
                    if (measu != null)
                        Measuremnts = measu;
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

        #region OnAddGoalCommand

        public DelegateCommand OnAddGoalCommand => new DelegateCommand(OnAddGoal);

        private async void OnAddGoal()
        {
            try
            {
                AddMode = false;
                if (LoggedInAccount.AccountGoal.Goal == Goal)
                    return;

                LoggedInAccount.AccountGoal.Goal = Goal;
                LoggedInAccount.AccountGoal.Start = Start;
                LoggedInAccount.AccountGoal.End = End;
                LoggedInAccount.AccountGoal = await _accountGoalService.AddAccountGoal(LoggedInAccount.AccountGoal);
                Goal = 0;
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnMeasurmentMoreCommand

        public DelegateCommand OnMeasurmentMoreCommand => new DelegateCommand(OnMore);

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
                            Invoked = () =>
                            {
                                AddMode = true;
                                Goal = LoggedInAccount.AccountGoal.Goal;
                            }
                        },
                        new DialogCommand
                        {
                            Label = AppResource.Profile_UpdateMeasurment,
                            Invoked = () => { NavigationService.Navigate("UpdateMeasurmentPage"); }
                        },
                        new DialogCommand
                        {
                            Label = AppResource.Cancel,
                            Invoked = () => { }
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

        #region OnRefreshPostsCommand

        public DelegateCommand OnRefreshPostsCommand => new DelegateCommand(OnRefreshPosts);

        private void OnRefreshPosts()
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

        #endregion
    }
}