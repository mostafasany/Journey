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
                var mode = parameters.GetValue<int>("Mode");
                IsAddMode = false;
                IsApproveRequestMode = false;

                if (mode == 0)
                {
                    //Add
                    IsAddMode = true;
                    ToChallenge = parameters.GetValue<Account>("ToChallenge") ?? null;
                    LoggedInAccount = await _accountService.GetAccountAsync();
                    if (string.IsNullOrEmpty(LoggedInAccount.ChallengeId))
                    {
                        SelectedChallenge = new Challenge();
                        var challengesAccount = new ObservableCollection<ChallengeAccount>();
                        challengesAccount.Add(new ChallengeAccount(LoggedInAccount));
                        challengesAccount.Add(new ChallengeAccount(ToChallenge));
                        SelectedChallenge.ChallengeAccounts = challengesAccount;
                    }
                }
                else
                {
                    var challengeId = parameters.GetValue<string>("Challenge") ?? null;
                    if (!string.IsNullOrEmpty(challengeId))
                        SelectedChallenge = await _challengeService.GetChallengeAsync(challengeId);

                    if (mode == 1)
                    {
                        //Edit
                    }
                    else if (mode == 2)
                    {
                        IsApproveRequestMode = true;
                        //Approve Request 
                    }
                }
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

        private bool isApproveRequestMode;

        public bool IsApproveRequestMode
        {
            get => isApproveRequestMode;
            set => SetProperty(ref isApproveRequestMode, value);
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
                var hasActiveChallange = await _challengeService.CheckAccountHasChallengeAsync();
                if (!hasActiveChallange)
                {
                    var challenge = await _challengeService.AddChallengeAsync(SelectedChallenge);
                    if (challenge != null)
                    {
                        await DialogService.ShowMessageAsync(AppResource.Challenge_ApproveMessage, "");
                        await NavigationService.Navigate("HomePage", false, "Sync");
                    }
                }
                else
                {
                    await DialogService.ShowMessageAsync(AppResource.Challenge_AlreadyExists, AppResource.Error);
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowGenericErrorMessageAsync(AppResource.Error, ex.Message);
            }
            finally
            {
                HideProgress();
            }
        }

        #endregion

        #region OnApproveRequestCommand

        public DelegateCommand OnApproveRequestCommand => new DelegateCommand(OnApproveRequest);


        private async void OnApproveRequest()
        {
            try
            {
                var challenge = await _challengeService.ApproveChallengeAsync(SelectedChallenge);
                if (challenge != null)
                    await NavigationService.Navigate("HomePage", true, "Sync");
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