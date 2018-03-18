using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Journey.Models.Challenge;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Challenge;
using Prism.Commands;
using Prism.Navigation;
using Unity;
using ChallengeAccount = Journey.Models.Challenge.ChallengeAccount;

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

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                FirstTabSelected = "#f1f1f1";
                SecondTabSelected = "#ffffff";
                ThirdTabSelected = "#ffffff";
                FourthTabSelected = "#ffffff";

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


        private string _winnerAccountInKm;

        public string WinnerAccountInKM
        {
            get => _winnerAccountInKm;
            set => SetProperty(ref _winnerAccountInKm, value);
        }

        private string _winnerAccountInExercises;

        public string WinnerAccountInExercises
        {
            get => _winnerAccountInExercises;
            set => SetProperty(ref _winnerAccountInExercises, value);
        }

        private Challenge _selectedChallenge;

        public Challenge SelectedChallenge
        {
            get => _selectedChallenge;
            set => SetProperty(ref _selectedChallenge, value);
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
                        ChallengeAccount challenge1 = SelectedChallenge.ChallengeAccounts.FirstOrDefault();
                        ChallengeAccount challenge2 = SelectedChallenge.ChallengeAccounts.LastOrDefault();
                        int challenge1ExerciseCount = ChallengeProgress.Count(a => a.WinnerAccountInExercises?.Name == challenge1.Name);
                        int challenge2ExerciseCount = ChallengeProgress.Count(a => a.WinnerAccountInExercises?.Name == challenge2.Name);
                        int challenge1KmCount = ChallengeProgress.Count(a => a.WinnerAccountInKM?.Name == challenge1.Name);
                        int challenge2KmCount = ChallengeProgress.Count(a => a.WinnerAccountInKM?.Name == challenge2.Name);
                        if (challenge1ExerciseCount != challenge2ExerciseCount)
                            WinnerAccountInExercises = challenge1ExerciseCount > challenge2ExerciseCount ? challenge1?.Name : challenge2?.Name;
                        else
                            WinnerAccountInExercises = AppResource.Draw;
                        if (challenge1KmCount != challenge2KmCount)
                            WinnerAccountInKM = challenge1KmCount > challenge2KmCount ? challenge1?.Name : challenge2?.Name;
                        else
                            WinnerAccountInKM = AppResource.Draw;
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

        private void OnRefreshPosts()
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
                bool isLogginIn = await _accountService.LoginFirstAsync();
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