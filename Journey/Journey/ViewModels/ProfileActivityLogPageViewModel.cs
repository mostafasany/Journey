using System;
using System.Collections.Generic;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Challenge;
using Journey.Services.Buisness.ChallengeActivity;
using Journey.Services.Buisness.Notification;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ProfileActivityLogPageViewModel : ProfilePageViewModel, INavigationAware
    {
        private readonly IChallengeActivityService _challengeActivityService;
        private readonly IAccountService _accountService;
        private readonly IChallengeService _challengeService;

        public ProfileActivityLogPageViewModel(IUnityContainer container, IAccountService accountService,INotificationService notificationService,
            IChallengeService challengeService, IChallengeActivityService challengeActivityService) :
        base(container, accountService,notificationService)
        {
            _challengeActivityService = challengeActivityService;
            _accountService = accountService;
            _challengeService = challengeService;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                FirstTabSelected = "#ffffff";
                SecondTabSelected = "#f1f1f1";
                ThirdTabSelected = "#ffffff";
                FourthTabSelected = "#ffffff";

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

        List<ChallengeActivityLog> _challengeActivityLog;
        public List<ChallengeActivityLog> ChallengeActivityLog
        {
            get => _challengeActivityLog;
            set => SetProperty(ref _challengeActivityLog, value);
        }

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                if (!string.IsNullOrEmpty(_accountService.LoggedInAccount.ChallengeId))
                {
                    ChallengeActivityLog = await _challengeActivityService.GetActivitsAsync(_accountService.LoggedInAccount.ChallengeId,-1,-1);
                }

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

        #endregion

        #region Commands

        #endregion
    }
}