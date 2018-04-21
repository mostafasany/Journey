using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Journey.Models.Account;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Friend;
using Journey.Services.Buisness.Notification;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class FriendsPageViewModel : MainNavigationViewModel, INavigationAware
    {
        private readonly IFriendService _friendService;
        private readonly IAccountService _accountService;

        public FriendsPageViewModel(IUnityContainer container, IAccountService accountService,
                                    INotificationService notificationService,
                                       IFriendService friendService)
            :
            base(container, accountService, notificationService)
        {
            _friendService = friendService;
            _accountService = accountService;
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

        private ObservableCollection<Account> _friendsList;

        public ObservableCollection<Account> FriendsList
        {
            get => _friendsList;
            set => SetProperty(ref _friendsList, value);
        }

        private Account _selectedFriend;

        public Account SelectedFriend
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
                List<Account> friends = await _accountService.FindAccontAsync(keyword);
                if (friends != null)
                    FriendsList = new ObservableCollection<Account>(friends);
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
                if (string.IsNullOrEmpty(selectedFriend?.Id))
                    return;


                var competeCommand = new DialogCommand
                {
                    Label = AppResource.Yes,
                    Invoked = async () =>
                    {
                        var status = await _friendService.FollowAsync(selectedFriend.Id);
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

                var message = string.Format(AppResource.Friends_AddNewFriend, selectedFriend.Name);
                await DialogService.ShowMessageAsync("", message, commands);
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
                await OnSearch(_searchKeyword);
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
                await OnSearch(_searchKeyword);

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

        #endregion
    }
}