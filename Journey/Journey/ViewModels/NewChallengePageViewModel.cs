using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Journey.Models.Account;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.PostComment;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class NewChallengePageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IPostCommentService _postCommentService;

        public NewChallengePageViewModel(IUnityContainer container,
            IPostCommentService postCommentService, IAccountService accountService) :
            base(container)
        {
            _postCommentService = postCommentService;
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
                ToChallenge=parameters.GetValue<Account>("ToChallenge") ?? null;
           
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
            set
            {
                SetProperty(ref loggedInAccount, value);
            }
        }

        bool isAddMode;
        public bool IsAddMode
        {
            get => isAddMode;
            set => SetProperty(ref isAddMode, value);
        }

        Account toChallenge;
        public Account ToChallenge
        {
            get => toChallenge;
            set => SetProperty(ref toChallenge, value);
        }

        Challenge selectedChallenge;
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

                if (!string.IsNullOrEmpty(LoggedInAccount.ChallengeId))
                {
                    SelectedChallenge = new Challenge();
                    ObservableCollection<Models.Challenge.ChallengeAccount> challengesAccount = new ObservableCollection<Models.Challenge.ChallengeAccount>();
                    challengesAccount.Add(new Models.Challenge.ChallengeAccount(LoggedInAccount));
                    challengesAccount.Add(new Models.Challenge.ChallengeAccount(ToChallenge));
                    SelectedChallenge.ChallengeAccounts = challengesAccount;
                }
                else
                {
                    //SelectedChallenge = await challengeService.GetChallengeAsync(LoggedInAccount.ChallengeId);
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