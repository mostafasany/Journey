using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Journey.Models.Account;
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
    public class NewChallengePageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IChallengeService _challengeService;

        public NewChallengePageViewModel(IUnityContainer container,
            IChallengeService challengeService, IAccountService accountService) :
            base(container)
        {
            _challengeService = challengeService;
            _accountService = accountService;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                ToChallenge = parameters.GetValue<Account>("ToChallenge") ?? null;

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

        private Account loggedInAccount;

        public Account LoggedInAccount
        {
            get => loggedInAccount;
            set => SetProperty(ref loggedInAccount, value);
        }

        private bool isAddMode;

        public bool IsAddMode
        {
            get => isAddMode;
            set => SetProperty(ref isAddMode, value);
        }

        private Account toChallenge;

        public Account ToChallenge
        {
            get => toChallenge;
            set => SetProperty(ref toChallenge, value);
        }

        private Challenge selectedChallenge;

        public Challenge SelectedChallenge
        {
            get => selectedChallenge;
            set => SetProperty(ref selectedChallenge, value);
        }

        //List<Interval> intervalList;
        //public List<Interval> IntervalList
        //{
        //    get => intervalList;
        //    set => Set(ref intervalList, value);
        //}

        //Interval selectedInterval;
        //public Interval SelectedInterval
        //{
        //    get => selectedInterval;
        //    set => Set(ref selectedInterval, value);
        //}

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                LoggedInAccount = await _accountService.GetAccountAsync();
                IsAddMode = string.IsNullOrEmpty(LoggedInAccount.ChallengeId);

                if (string.IsNullOrEmpty(LoggedInAccount.ChallengeId))
                {
                    SelectedChallenge = new Challenge();
                    var challengesAccount = new ObservableCollection<ChallengeAccount>();
                    challengesAccount.Add(new ChallengeAccount(LoggedInAccount));
                    challengesAccount.Add(new ChallengeAccount(ToChallenge));
                    SelectedChallenge.ChallengeAccounts = challengesAccount;
                }
                else
                {
                    SelectedChallenge = await _challengeService.GetChallengeAsync(LoggedInAccount.ChallengeId);
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

        #region OnStartChallengeCommand

        public DelegateCommand OnStartChallengeCommand => new DelegateCommand(OnStartChallenge);

        private async void OnStartChallenge()
        {
            try
            {
                if (IsProgress())
                    return;

                string[] Options = { };
                if (SelectedChallenge.StartDate >= SelectedChallenge.EndDate)
                {
                    await DialogService.ShowMessageAsync(AppResource.Error, AppResource.Challenge_DataValidation);
                    return;
                }

                var startChallengeCommand = new DialogCommand
                {
                    Label = AppResource.Yes,
                    Invoked = () => StartChallenge()
                };

                var cancelCommand = new DialogCommand
                {
                    Label = AppResource.Cancel
                };


                var commands = new List<DialogCommand>
                {
                    startChallengeCommand,
                    cancelCommand
                };
                //SelectedChallenge.Interval = SelectedInterval.IntervalValue;
                await DialogService.ShowMessageAsync("", AppResource.Challenge_Start, commands);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
                await DialogService.ShowGenericErrorMessageAsync(AppResource.Error, ex.Message);
            }
            finally
            {
                HideProgress();
            }
        }

        private async Task StartChallenge()
        {
            try
            {
                if (IsProgress())
                    return;

                ShowProgress();
                SelectedChallenge.IsActive = true;
                var existingChallenge = await _challengeService.GetAccountChallengeAsync();
                if (existingChallenge == null)
                {
                    var challenge = await _challengeService.SaveCurrentChallengeAsync(SelectedChallenge);
                    if (challenge != null)
                        await NavigationService.Navigate("HomePage", true, "Sync");
                }
                else
                {
                    await DialogService.ShowMessageAsync(AppResource.Error, AppResource.Challenge_AlreadyExists);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                HideProgress();
            }
        }

        #endregion

        #region OnBackCommand

        public DelegateCommand OnBackCommand => new DelegateCommand(OnBack);


        private async void OnBack()
        {
            try
            {
                NavigationService.GoBack();
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