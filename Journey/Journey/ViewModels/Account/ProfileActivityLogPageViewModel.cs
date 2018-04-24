using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Challenge;
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
        private readonly IChallengeService _challengeService;
        private readonly ILocationService _locationService;
        private readonly ISettingsService _settingsService;
        private const string LastLogDateTime = "LastLogDateTime";
        private const string LastLogId = "LastLogId";

        private const string LastKcalLogDateTime = "LastKcalLogDateTime";
        private const string LastKcalLogId = "LastKcalLogId";

        public ProfileActivityLogPageViewModel(IUnityContainer container, IAccountService accountService,
            INotificationService notificationService,
            ISettingsService settingsService,
            ILocationService locationService,
            IChallengeService challengeService,
            IChallengeActivityService challengeActivityService) :
            base(container, accountService, notificationService)
        {
            _locationService = locationService;
            _challengeService = challengeService;
            _challengeActivityService = challengeActivityService;
            _accountService = accountService;
            _settingsService = settingsService;
            SubscribeHealthService();
            //_settingsService.Remove(LastLogDateTime);
            //_settingsService.Remove(LastLogId);
            //_settingsService.Remove(LastKcalLogDateTime);
            //_settingsService.Remove(LastKcalLogId);
        }

        private async void SubscribeHealthService()
        {
            var healthService = DependencyService.Get<IHealthService>();
            if (healthService == null)
                return;
            bool status = await healthService.Authenticate();
            if (status)
            {
                HasHealthApi = true;
                healthService.HealthDataChanged += HealthService_HealthDataChanged;
            }
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
                    LogKmActivity(double.Parse(parsedDistance.ToString("0.##")));
                }

                if (unit == Unit.KCAL.ToString())
                {
                    string measure = e.Data["Measure"];
                    LogKcalActivity(double.Parse(measure));
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
                if (!string.IsNullOrEmpty(_accountService?.LoggedInAccount?.ChallengeId))
                    challenges = await _challengeActivityService.GetChallengeActivitiesAsync(_accountService.LoggedInAccount.ChallengeId);
                else
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

        private async void LogKmActivity(double km)
        {
            string logDateTime = await _settingsService.Get(LastLogDateTime);
            if (string.IsNullOrEmpty(logDateTime))
            {
                AddLogKmActivity(km);
            }
            else
            {
                DateTime parsedLogDate = DateTime.Parse(logDateTime);
                if (DateTime.Now.Date == parsedLogDate.Date)
                    UpdateLogKmActivity(km);
                else
                    AddLogKmActivity(km);
            }
        }

        private async void AddLogKmActivity(double km)
        {
            DateTime currentDateTime = DateTime.Now;
            var newActivity = new ChallengeKmActivityLog
            {
                Account = _accountService.LoggedInAccount,
                Challenge = _accountService.LoggedInAccount.ChallengeId,
                DatetTime = currentDateTime,
                KM = km
            };
            ChallengeActivityLog activity = await _challengeActivityService.AddActivityAsync(newActivity);

            await _settingsService.Set(LastLogId, activity.Id);
            await _settingsService.Set(LastLogDateTime, currentDateTime.ToString(CultureInfo.InvariantCulture));
            if (ChallengeActivityLog.Any())
                ChallengeActivityLog.Insert(0, activity);
            else
                ChallengeActivityLog.Add(activity);
        }

        private async void UpdateLogKmActivity(double km)
        {
            DateTime currentDateTime = DateTime.Now;
            string lastLogId = await _settingsService.Get(LastLogId);
            await _challengeActivityService.UpdateActivityAsync(new ChallengeKmActivityLog
            {
                Id = lastLogId,
                Account = _accountService.LoggedInAccount,
                Challenge = _accountService.LoggedInAccount.ChallengeId,
                DatetTime = currentDateTime,
                KM = km
            });
            await _settingsService.Set(LastLogDateTime, currentDateTime.ToString(CultureInfo.InvariantCulture));
            if (ChallengeActivityLog.FirstOrDefault(a => a.Id == lastLogId) is ChallengeKmActivityLog updatedItem)
                updatedItem.KM = km;
        }

        private async void LogKcalActivity(double kcal)
        {
            string logDateTime = await _settingsService.Get(LastKcalLogDateTime);
            if (string.IsNullOrEmpty(logDateTime))
            {
                AddLogKcalActivity(kcal);
            }
            else
            {
                DateTime parsedLogDate = DateTime.Parse(logDateTime);
                if (DateTime.Now.Date == parsedLogDate.Date)
                    UpdateLogKcalActivity(kcal);
                else
                    AddLogKcalActivity(kcal);
            }
        }

        private async void AddLogKcalActivity(double kcal)
        {
            DateTime currentDateTime = DateTime.Now;
            var newActivity = new ChallengeKcalActivityLog
            {
                Account = _accountService.LoggedInAccount,
                Challenge = _accountService.LoggedInAccount.ChallengeId,
                DatetTime = currentDateTime,
                Kcal = kcal
            };
            ChallengeActivityLog activity = await _challengeActivityService.AddActivityAsync(newActivity);

            await _settingsService.Set(LastKcalLogId, activity.Id);
            await _settingsService.Set(LastKcalLogDateTime, currentDateTime.ToString(CultureInfo.InvariantCulture));
            if (ChallengeActivityLog.Any())
                ChallengeActivityLog.Insert(0, activity);
            else
                ChallengeActivityLog.Add(activity);
        }

        private async void UpdateLogKcalActivity(double kcal)
        {
            DateTime currentDateTime = DateTime.Now;
            string lastLogId = await _settingsService.Get(LastKcalLogId);
            await _challengeActivityService.UpdateActivityAsync(new ChallengeKcalActivityLog
            {
                Id = lastLogId,
                Account = _accountService.LoggedInAccount,
                Challenge = _accountService.LoggedInAccount.ChallengeId,
                DatetTime = currentDateTime,
                Kcal = kcal
            });
            await _settingsService.Set(LastKcalLogDateTime, currentDateTime.ToString(CultureInfo.InvariantCulture));
            if (ChallengeActivityLog.FirstOrDefault(a => a.Id == lastLogId) is ChallengeKcalActivityLog updatedItem)
                updatedItem.Kcal = kcal;
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
                ChallengeActivityLog newActivity = await _challengeActivityService.AddExerciseActivityAsync(myLocation);
                if (newActivity == null)
                    return;
                if (ChallengeActivityLog.Any())
                    ChallengeActivityLog.Insert(0, newActivity);
                else
                    ChallengeActivityLog.Add(newActivity);
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

        private async void OnPullRefreshRequest()
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