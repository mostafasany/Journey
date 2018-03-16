using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Abstractions.Services.Contracts;
using Journey.Models.Account;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Friend;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ChooseChallengeFriendPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IFriendService _friendService;

        public ChooseChallengeFriendPageViewModel(IUnityContainer container, IAccountService accountService,
            IFriendService friendService)
            : base(container)
        {
            _accountService = accountService;
            _friendService = friendService;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                Intialize();
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        #endregion

        #region Properties

        private ObservableCollection<Account> friendsList;

        public ObservableCollection<Account> FriendsList
        {
            get => friendsList;
            set => SetProperty(ref friendsList, value);
        }

        private Account selectedFriend;

        public Account SelectedFriend
        {
            get => selectedFriend;
            set => SetProperty(ref selectedFriend, value);
        }

        private string searchKeyword;

        public string SearchKeyword
        {
            get => searchKeyword;
            set => SetProperty(ref searchKeyword, value);
        }

        private bool isPullRefreshLoading;

        public bool IsPullRefreshLoading
        {
            get => isPullRefreshLoading;
            set => SetProperty(ref isPullRefreshLoading, value);
        }

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();

                IsPullRefreshLoading = false;

                SelectedFriend = null;
                var friends = await _friendService.GetFriendsAsync("");
                if (friends != null)
                    FriendsList = new ObservableCollection<Account>(friends);
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

        #region OnSelectedFriendCommand

        public DelegateCommand<Account> OnSelectedFriendCommand => new DelegateCommand<Account>(OnSelectedFriend);

        private async void OnSelectedFriend(Account selectedFriend)
        {
            try
            {
                if (string.IsNullOrEmpty(selectedFriend?.Id))
                    return;


                var competeCommand = new DialogCommand
                {
                    Label = AppResource.Yes,
                    Invoked = async () =>
                    {
                        var parameters = new Dictionary<string, object>
                        {
                            {"ToChallenge", selectedFriend},
                            {"Mode", 0}
                        };
                        await NavigationService.Navigate("NewChallengePage", parameters);
                    }
                };

                var cancelCommand = new DialogCommand
                {
                    Label = AppResource.Cancel
                };

                var commands = new List<DialogCommand>
                {
                    competeCommand,
                    cancelCommand
                };

                await DialogService.ShowMessageAsync("", AppResource.Challenge_Comepete, commands);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnSearchCommand

        public DelegateCommand OnSearchCommand => new DelegateCommand(OnSearch);

        private async void OnSearch()
        {
            try
            {
                var friends = await _friendService.GetFriendsAsync(searchKeyword);
                if (friends != null)
                    FriendsList = new ObservableCollection<Account>(friends);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnPullRefreshRequestCommand

        public DelegateCommand OnPullRefreshRequestCommand => new DelegateCommand(OnPullRefreshRequest);

        private async void OnPullRefreshRequest()
        {
            try
            {
                IsPullRefreshLoading = true;
                ShowProgress();
                var friends = await _friendService.GetFriendsAsync(searchKeyword);
                if (friends != null)
                    FriendsList = new ObservableCollection<Account>(friends);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
            finally
            {
                IsPullRefreshLoading = false;
                HideProgress();
            }
        }

        #endregion

        #region OnBackCommand

        public DelegateCommand OnBackCommand => new DelegateCommand(OnBack);


        private void OnBack()
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