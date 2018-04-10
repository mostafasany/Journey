using System;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Notification;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class StartNewChallengePageViewModel : MainNavigationViewModel, INavigationAware
    {
        public StartNewChallengePageViewModel(IUnityContainer container,
                                              IAccountService accountService,
                                              INotificationService notificationService) :
            base(container, accountService, notificationService)
        {
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                
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


        #endregion

        #region Methods

        public override void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
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

        #region OnStartNewChallengeCommand

        public DelegateCommand OnStartNewChallengeCommand => new DelegateCommand(OnStartNewChallenge);

        private void OnStartNewChallenge()
        {
            NavigationService.Navigate("ChooseChallengeFriendPage");
        }

        #endregion

        #endregion
    }
}