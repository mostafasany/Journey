using System;
using System.Windows.Input;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Challenge;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ProfileChallengePageViewModel : ProfilePageViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IChallengeService _challengeService;

        public ProfileChallengePageViewModel(IUnityContainer container, IAccountService accountService,
            IChallengeService challengeService) :
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

        private bool _hasActiveChallenge;

        public bool HasActiveChallenge
        {
            get => _hasActiveChallenge;
            set
            {
                SetProperty(ref _hasActiveChallenge, value);
                RaisePropertyChanged(nameof(HasNotActiveChallenge));
            }
        }

        public bool HasNotActiveChallenge => !_hasActiveChallenge;

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                if (!string.IsNullOrEmpty(_accountService.LoggedInAccount.ChallengeId))
                {
                    SelectedChallenge =
                        await _challengeService.GetChallengeAsync(_accountService.LoggedInAccount.ChallengeId);
                    if (SelectedChallenge != null)
                        HasActiveChallenge = true;
                    else
                        HasActiveChallenge = false;
                }
                else
                {
                    HasActiveChallenge = false;
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

        #region OnStartNewChallengeCommand

        private ICommand _onStartNewChallengeCommand;

        public ICommand OnStartNewChallengeCommand => _onStartNewChallengeCommand ??
                                                      (_onStartNewChallengeCommand =
                                                          new DelegateCommand(OnStartNewChallenge));

        private async void OnStartNewChallenge()
        {
            try
            {
                var isLogginIn = await _accountService.LoginFirstAsync();
                if (isLogginIn)
                    await NavigationService.Navigate("ChooseChallengeFriendPage");
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