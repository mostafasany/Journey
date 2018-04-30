using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.ChallengeActivity;
using Journey.Services.Buisness.Notification;
using Prism.Commands;
using Prism.Navigation;
using Unity;
using Xamarin.Forms;

namespace Journey.ViewModels
{
    public class ProfileActivityLogPageViewModel : ProfilePageViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IChallengeActivityService _challengeActivityService;
        private readonly ILocationService _locationService;

        public ProfileActivityLogPageViewModel(IUnityContainer container, IAccountService accountService,
            INotificationService notificationService,
            ILocationService locationService,
            IChallengeActivityService challengeActivityService) :
            base(container, accountService, notificationService)
        {
            _locationService = locationService;
            _challengeActivityService = challengeActivityService;
            _accountService = accountService;
            SubscribeHealthService();
        }

        #region Events

        private void HealthService_HealthDataChanged(object sender, HealthDataEventArgs e)
        {
            try
            {
                if (e?.Data == null)
                    return;

                string unit = e.Data["Unit"];
                if (unit == Unit.RunningWalking.ToString())
                {
                    string measure = e.Data["Measure"];
                    decimal parsedDistance = decimal.Parse(measure) / 1000;
                    AddUpdateLogKmActivity(double.Parse(parsedDistance.ToString("0.##")));
                }

                if (unit == Unit.KCAL.ToString())
                {
                    string measure = e.Data["Measure"];
                    AddUpdateLogKcalActivity(double.Parse(measure));
                }
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                ClearTabSelection();
                FirstTabSelected = "#f1f1f1";


                if (parameters?.GetNavigationMode() == NavigationMode.Back)
                {
                    var location = parameters.GetValue<Location>("Location");
                    if (location != null)
                        AddExerciseActivity(location);
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

        private ObservableCollection<ChallengeActivityLog> _challengeActivityLog;

        public ObservableCollection<ChallengeActivityLog> ChallengeActivityLog
        {
            get => _challengeActivityLog;
            set => SetProperty(ref _challengeActivityLog, value);
        }


        private bool _hasHealthApi;

        public bool HasHealthApi
        {
            get => _hasHealthApi;
            set => SetProperty(ref _hasHealthApi, value);
        }

        private bool _isPullRefreshLoading;

        public bool IsPullRefreshLoading
        {
            get => _isPullRefreshLoading;
            set => SetProperty(ref _isPullRefreshLoading, value);
        }

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                List<ChallengeActivityLog> challenges;
                //if (!string.IsNullOrEmpty(_accountService?.LoggedInAccount?.ChallengeId))
                //    challenges = await _challengeActivityService.GetChallengeActivitiesAsync(_accountService.LoggedInAccount.ChallengeId);
                //else
                challenges = await _challengeActivityService.GetAccountActivitiesAsync();
                if (challenges != null)
                    ChallengeActivityLog = new ObservableCollection<ChallengeActivityLog>(challenges);
                else
                    ChallengeActivityLog = new ObservableCollection<ChallengeActivityLog>();
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

        private async void SubscribeHealthService()
        {
            var healthService = DependencyService.Get<IHealthService>();
            if (healthService == null)
                return;
            bool status = await healthService.AuthenticateAsync();
            if (status)
            {
                HasHealthApi = true;
                healthService.HealthDataChanged -= HealthService_HealthDataChanged;
                healthService.HealthDataChanged += HealthService_HealthDataChanged;
            }
        }

        private async void AddUpdateLogKmActivity(double km)
        {
            if (km == 0)
                return;
            var newActivity = new ChallengeKmActivityLog
            {
                Account = _accountService.LoggedInAccount,
                Challenge = _accountService.LoggedInAccount.ChallengeId,
                KM = km
            };
            await AddUpdateActivity(newActivity);
        }

        private async void AddUpdateLogKcalActivity(double kcal)
        {
            if (kcal == 0)
                return;
            var newActivity = new ChallengeKcalActivityLog
            {
                Account = _accountService.LoggedInAccount,
                Challenge = _accountService.LoggedInAccount.ChallengeId,
                DatetTime = DateTime.Now,
                Kcal = kcal
            };
            await AddUpdateActivity(newActivity);
        }

        private async Task AddUpdateActivity(ChallengeActivityLog activityLog)
        {
            await _challengeActivityService.AddUpdateActivityAsync(activityLog);
            Intialize();
        }

        #endregion

        #region Commands

        #region OnExerciseCommand

        private ICommand _onExerciseCommand;

        public ICommand OnExerciseCommand => _onExerciseCommand ?? (
                                                 _onExerciseCommand =
                                                     new DelegateCommand(OnExercise));

        private async void OnExercise()
        {
            try
            {
                if (IsProgress())
                    return;

                ShowProgress();

                Location myLocation = await _locationService.ObtainMyLocationAsync();
                Challenge challenge = await _challengeActivityService.IsExercisingInChallengeWorkoutPlaceAsync(myLocation);
                if (challenge == null)
                    await NavigationService.Navigate("ChooseLocationPage");
                else
                    AddExerciseActivity(challenge.SelectedLocation, challenge.Id);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleAndShowDialog(ex);
            }
            finally
            {
                HideProgress();
            }
        }

        private async void AddExerciseActivity(Location myLocation, string challenge = null)
        {
            ChallengeActivityLog newActivity = await _challengeActivityService.AddExerciseActivityAsync(myLocation, challenge);
            if (newActivity == null)
                return;
            //if (ChallengeActivityLog.Any())
            //    ChallengeActivityLog.Insert(0, newActivity);
            //else
            //ChallengeActivityLog.Add(newActivity);
            Intialize();
        }

        #endregion

        #region OnLogKmCommand

        private ICommand _onLogKmCommand;

        public ICommand OnLogKmCommand => _onLogKmCommand ?? (
                                              _onLogKmCommand =
                                                  new DelegateCommand(LogKm));

        private async void LogKm()
        {
            try
            {
                var healthService = DependencyService.Get<IHealthService>();
                await healthService.GetRunningWalkingDistanceAsync();
                await healthService.GetCaloriesAsync();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleAndShowDialog(ex);
            }
        }

        #endregion

        #region OnPullRefreshRequestCommand

        public ICommand OnPullRefreshRequestCommand => new DelegateCommand(OnPullRefreshRequest);

        private void OnPullRefreshRequest()
        {
            try
            {
                IsPullRefreshLoading = true;

                ShowProgress();
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