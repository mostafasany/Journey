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

        public FriendsPageViewModel(IUnityContainer container, IAccountService accountService,
            INotificationService notificationService,
            IFriendService friendService)
            :
            base(container, accountService, notificationService) => _friendService = friendService;


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
                List<FriendShip> friends = await _friendService.FindAccontAsync(keyword);
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

        public DelegateCommand<FriendShip> OnSelectedFriendCommand => new DelegateCommand<FriendShip>(OnSelectedFriend);

        private void OnSelectedFriend(FriendShip selectedFriend)
        {
            try
            {
                if (selectedFriend == null || string.IsNullOrEmpty(selectedFriend.Id))
                    return;

                if (selectedFriend.FriendShipEnum == FriendShipEnum.Requested)
                    RequestIgnore(selectedFriend);
                else if (selectedFriend.FriendShipEnum == FriendShipEnum.Nothing)
                    RequestFollow(selectedFriend);
                else if (selectedFriend.FriendShipEnum == FriendShipEnum.Approved)
                    RequestUnFollow(selectedFriend);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        private async void RequestFollow(FriendShip selectedFriend)
        {
            var competeCommand = new DialogCommand
            {
                Label = AppResource.Yes,
                Invoked = async () =>
                {
                    bool status = await _friendService.FollowRequestAsync(selectedFriend.Id);
                    if (status)
                        selectedFriend.FriendShipStatus = ((int) FriendShipEnum.Requested).ToString();
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

            string message = string.Format(AppResource.Friends_FollowFriend, selectedFriend.Name);
            await DialogService.ShowMessageAsync("", message, commands);
        }

        private async void RequestIgnore(FriendShip selectedFriend)
        {
            var competeCommand = new DialogCommand
            {
                Label = AppResource.Yes,
                Invoked = async () =>
                {
                    bool status = await _friendService.IgnoreApproveAsync(selectedFriend.FriendShipId);
                    if (status)
                        selectedFriend.FriendShipStatus = ((int) FriendShipEnum.Nothing).ToString();
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

            string message = string.Format(AppResource.Friends_IgnoreFriend, selectedFriend.Name);
            await DialogService.ShowMessageAsync("", message, commands);
        }

        private async void RequestUnFollow(FriendShip selectedFriend)
        {
            var competeCommand = new DialogCommand
            {
                Label = AppResource.Yes,
                Invoked = async () =>
                {
                    bool status = await _friendService.FollowRejectAsync(selectedFriend.FriendShipId);
                    if (status)
                        selectedFriend.FriendShipStatus = ((int) FriendShipEnum.Nothing).ToString();
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

            string message = string.Format(AppResource.Friends_UnFollowFriend, selectedFriend.Name);
            await DialogService.ShowMessageAsync("", message, commands);
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