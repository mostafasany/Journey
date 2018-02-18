using System;
using System.Collections.ObjectModel;
using Journey.Models.Account;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account;
using Prism.Commands;
using Prism.Navigation;
using Tawasol.Services;
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

        public override async void Intialize()
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
                base.Intialize();
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

        #region OnNewCommentCommand

        public DelegateCommand OnNewCommentCommand => new DelegateCommand(OnNewComment);

        private async void OnNewComment()
        {
            try
            {
                if (IsProgress())
                    return;
                ShowProgress();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleAndShowDialog(ex);
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