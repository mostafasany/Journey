using System;
using System.Collections.Generic;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.ChallengeActivity;
using Journey.Services.Buisness.Notification;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ProfileActivityLogPageViewModel : ProfilePageViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IChallengeActivityService _challengeActivityService;

        public ProfileActivityLogPageViewModel(IUnityContainer container, IAccountService accountService, INotificationService notificationService,
            IChallengeActivityService challengeActivityService) :
            base(container, accountService, notificationService)
        {
            _challengeActivityService = challengeActivityService;
            _accountService = accountService;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                ClearTabSelection();
                ThirdTabSelected = "#f1f1f1";


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

        private List<ChallengeActivityLog> _challengeActivityLog;

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
                if (!string.IsNullOrEmpty(_accountService.LoggedInAccount.ChallengeId)) ChallengeActivityLog = await _challengeActivityService.GetActivitsAsync(_accountService.LoggedInAccount.ChallengeId);

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