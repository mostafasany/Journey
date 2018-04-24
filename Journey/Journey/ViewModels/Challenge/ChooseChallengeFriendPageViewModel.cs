using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Journey.Models.Account;
using Journey.Resources;
using Journey.Services.Buisness.Friend;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ChooseChallengeFriendPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IFriendService _friendService;

        public ChooseChallengeFriendPageViewModel(IUnityContainer container,
            IFriendService friendService)
            : base(container) => _friendService = friendService;

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

        private ObservableCollection<FriendShip> _friendsList;

        public ObservableCollection<FriendShip> FriendsList
        {
            get => _friendsList;
            set => SetProperty(ref _friendsList, value);
        }

        private FriendShip _selectedFriend;

        public FriendShip SelectedFriend
        {
            get => _selectedFriend;
            set => SetProperty(ref _selectedFriend, value);
        }

        private string _searchKeyword;

        public string SearchKeyword
        {
            get => _searchKeyword;
            set => SetProperty(ref _searchKeyword, value);
        }

        private bool _isPullRefreshLoading;

        public bool IsPullRefreshLoading
        {
            get => _isPullRefreshLoading;
            set => SetProperty(ref _isPullRefreshLoading, value);
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
                await OnSearch("");
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

        private async Task OnSearch(string keyword)
        {
            try
            {
                List<FriendShip> friends = await _friendService.GetFriendsForChallengeAsync(keyword);
                if (friends != null)
                    FriendsList = new ObservableCollection<FriendShip>(friends);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
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
                if (selectedFriend == null || string.IsNullOrEmpty(selectedFriend.Id))
                    return;

                if (selectedFriend.HasActiveChallenge)
                {
                    await DialogService.ShowMessageAsync("", AppResource.Challenge_FriendHasActiveChallenge);
                    return;
                }

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
                await OnSearch(SearchKeyword);
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
                await OnSearch(SearchKeyword);
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