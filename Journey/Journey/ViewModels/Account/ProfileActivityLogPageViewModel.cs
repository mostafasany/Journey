using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Abstractions.Services.Contracts;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.ChallengeActivity;
using Journey.Services.Buisness.Notification;
using Prism.Navigation;
using Unity;
using Xamarin.Forms;

namespace Journey.ViewModels
{
    public class ProfileActivityLogPageViewModel : ProfilePageViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IChallengeActivityService _challengeActivityService;
        private readonly ISettingsService _settingsService;
        private const string LastLogDateTime = "LastLogDateTime";
        private const string LastLogId = "LastLogId";
        public ProfileActivityLogPageViewModel(IUnityContainer container, IAccountService accountService,
                                               INotificationService notificationService, ISettingsService settingsService,
            IChallengeActivityService challengeActivityService) :
            base(container, accountService, notificationService)
        {
            _challengeActivityService = challengeActivityService;
            _accountService = accountService;
            _settingsService = settingsService;
            SubscribeHealthService();
        }

        private async void SubscribeHealthService()
        {
            var healthService = DependencyService.Get<IHealthService>();
            if (healthService == null)
                return;
            var status = await healthService.Authenticate();
            if (status)
            {
                HasHealthApi = true;
                healthService.HealthDataChanged += HealthService_HealthDataChanged;
            }
        }

        #region Events

        void HealthService_HealthDataChanged(object sender, HealthDataEventArgs e)
        {
            try
            {
                if (e?.Data == null)
                    return;

                var unit = e.Data["Unit"];
                if (unit == Unit.RunningWalking.ToString())
                {
                    var measure = e.Data["Measure"];
                    var parsedDistance = decimal.Parse(measure) / 1000;
                    LogKmActivity(double.Parse(parsedDistance.ToString("0.##")));
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

        ObservableCollection<ChallengeActivityLog> _challengeActivityLog;

        public ObservableCollection<ChallengeActivityLog> ChallengeActivityLog
        {
            get => _challengeActivityLog;
            set => SetProperty(ref _challengeActivityLog, value);
        }


        bool _hasHealthApi;

        public bool HasHealthApi
        {
            get => _hasHealthApi;
            set => SetProperty(ref _hasHealthApi, value);
        }

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                List<ChallengeActivityLog> challenges;
                if (!string.IsNullOrEmpty(_accountService.LoggedInAccount.ChallengeId))
                {
                    challenges = await _challengeActivityService.GetChallengeActivitiesAsync(_accountService.LoggedInAccount.ChallengeId);
                }
                else
                {
                    challenges = await _challengeActivityService.GetAccountActivitiesAsync();
                }
                if (challenges != null)
                    ChallengeActivityLog = new ObservableCollection<ChallengeActivityLog>(challenges);
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
            var logDateTime = await _settingsService.Get(LastLogDateTime);
            if (string.IsNullOrEmpty(logDateTime))
            {
                AddLogKmActivity(km);
            }
            else
            {
                var parsedLogDate = DateTime.Parse(logDateTime);
                if (DateTime.Now.Date == parsedLogDate.Date)
                {
                    UpdateLogKmActivity(km);
                }
                else
                {
                    AddLogKmActivity(km);
                }
            }
        }

        private async void AddLogKmActivity(double km)
        {
            var currentDateTime = DateTime.Now;
            var newActivity = new ChallengeKmActivityLog
            {
                Account = _accountService.LoggedInAccount,
                Challenge = _accountService.LoggedInAccount.ChallengeId,
                DatetTime = currentDateTime,
                KM = km,
            };
            var activity = await _challengeActivityService.AddActivityAsync(newActivity);
            await _settingsService.Set(LastLogId, activity.Id);
            await _settingsService.Set(LastLogDateTime, currentDateTime.ToString(CultureInfo.InvariantCulture));
            if (ChallengeActivityLog.Any())
            {
                ChallengeActivityLog.Insert(0, activity);
            }
            else
            {
                ChallengeActivityLog.Add(activity);
            }

        }

        private async void UpdateLogKmActivity(double km)
        {
            var currentDateTime = DateTime.Now;
            var lastLogId = await _settingsService.Get(LastLogId);
            await _challengeActivityService.UpdateActivityAsync(new ChallengeKmActivityLog()
            {
                Id = lastLogId,
                Account = _accountService.LoggedInAccount,
                Challenge = _accountService.LoggedInAccount.ChallengeId,
                DatetTime = currentDateTime,
                KM = km,
            });
            await _settingsService.Set(LastLogDateTime, currentDateTime.ToString(CultureInfo.InvariantCulture));
            if (ChallengeActivityLog.FirstOrDefault(a => a.Id == lastLogId) is ChallengeKmActivityLog updatedItem)
                updatedItem.KM = km;
        }

        #endregion

        #region Commands

        #region OnViewChallengeCommand

        private ICommand _onLogKmCommand;


        public ICommand OnLogKmCommand => _onLogKmCommand ?? (
            _onLogKmCommand =
            new Prism.Commands.DelegateCommand(LogKm));

        private async void LogKm()
        {
            try
            {
                var healthService = DependencyService.Get<IHealthService>();
                // await healthService.GetAgeAsync();
                // await healthService.GetHeightAsync();
                // await healthService.GetWeightAsync();
                //await healthService.GetCaloriesAsync();
                //await healthService.GetStepsAsync();
                await healthService.GetRunningWalkingDistanceAsync();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleAndShowDialog(ex);
            }
        }

        #endregion

        #endregion
    }
}