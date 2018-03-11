using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Abstractions.Models;
using Journey.Models.Account;
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


        private List<ObservableChallengeProgressGroupCollection<AccountChallengeProgress>> _challengeProgress;

        public List<ObservableChallengeProgressGroupCollection<AccountChallengeProgress>> ChallengeProgress
        {
            get => _challengeProgress;
            set => SetProperty(ref _challengeProgress, value);
        }


        private Account _winnerAccountInKM;

        public Account WinnerAccountInKM
        {
            get => _winnerAccountInKM;
            set => SetProperty(ref _winnerAccountInKM, value);
        }
        private Account _winnerAccountInExercises;

        public Account WinnerAccountInExercises
        {
            get => _winnerAccountInExercises;
            set => SetProperty(ref _winnerAccountInExercises, value);
        }

        private Challenge selectedChallenge;

        public Challenge SelectedChallenge
        {
            get => selectedChallenge;
            set => SetProperty(ref selectedChallenge, value);
        }

        private bool _hasActiveChallenge = true;

        public bool HasActiveChallenge
        {
            get => _hasActiveChallenge;
            set => SetProperty(ref _hasActiveChallenge, value);

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
                    SelectedChallenge = await _challengeService.GetChallengeAsync(_accountService.LoggedInAccount.ChallengeId);
                    if (SelectedChallenge != null)
                    {
                        HasActiveChallenge = true;
                        ChallengeProgress = await _challengeService.GetChallengePorgessAsync(SelectedChallenge.Id);
                        var challenge1 = SelectedChallenge.ChallengeAccounts.FirstOrDefault();
                        var challenge2 = SelectedChallenge.ChallengeAccounts.LastOrDefault();
                        var challenge1ExerciseCount = ChallengeProgress.Count(a => a.WinnerAccountInExercises?.Name == challenge1.Name);
                        var challenge2ExerciseCount = ChallengeProgress.Count(a => a.WinnerAccountInExercises?.Name == challenge2.Name);
                        var challenge1KMCount = ChallengeProgress.Count(a => a.WinnerAccountInKM?.Name == challenge1.Name);
                        var challenge2KMCount = ChallengeProgress.Count(a => a.WinnerAccountInKM?.Name == challenge2.Name);
                        if (challenge1ExerciseCount != challenge2ExerciseCount)
                            WinnerAccountInExercises = challenge1ExerciseCount > challenge2ExerciseCount ? challenge1 : challenge2;
                        if (challenge1KMCount != challenge2KMCount)
                            WinnerAccountInKM = challenge1KMCount > challenge2KMCount ? challenge1 : challenge2;
                    }
                    else
                    {
                        HasActiveChallenge = false;
                    }
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