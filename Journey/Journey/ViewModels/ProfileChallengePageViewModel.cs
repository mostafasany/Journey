using System;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account;
using Prism.Commands;
using Prism.Navigation;
using Tawasol.Services;
using Unity;

namespace Journey.ViewModels
{
    public class ProfileChallengePageViewModel : ProfilePageViewModel, INavigationAware
    {

        private readonly IAccountService _accountService;
        private readonly IChallengeService _challengeService;
        public ProfileChallengePageViewModel(IUnityContainer container, IAccountService accountService, IChallengeService challengeService) :
        base(container, accountService)
        {
            _accountService = accountService;
            _challengeService = challengeService;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                if (parameters.GetNavigationMode() == NavigationMode.Back)
                {

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


        private Challenge selectedChallenge;

        public Challenge SelectedChallenge
        {
            get => selectedChallenge;
            set => SetProperty(ref selectedChallenge, value);
        }

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                SelectedChallenge = await _challengeService.GetAccountChallengeAsync();
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

        #region OnRefreshPostsCommand

        public DelegateCommand OnRefreshPostsCommand => new DelegateCommand(OnRefreshPosts);

        private async void OnRefreshPosts()
        {
            try
            {

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