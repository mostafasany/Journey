using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Abstractions.Forms;
using Abstractions.Models;
using Journey.Models.Account;
using Journey.Models.Post;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Notification;
using Journey.Services.Buisness.Post;
using Journey.ViewModels.Wall;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class MainNavigationViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly INotificationService _notificationService;

        private Account _loggedInAccount;

        private int _notificationsCount;

        public MainNavigationViewModel(IUnityContainer container, IAccountService accountService, INotificationService notificationService) : base(container)
        {
            _notificationService = notificationService;
            _accountService = accountService;
            LoadAccount();
            LoadNotificationsCount();
        }

        public Media Image => LoggedInAccount == null
            ? new Media {Path = "http://bit.ly/2zBffZy"}
            : _loggedInAccount.Image;

        public Account LoggedInAccount
        {
            get => _loggedInAccount;
            set
            {
                SetProperty(ref _loggedInAccount, value);
                RaisePropertyChanged(nameof(Image));
            }
        }

        public int NotificationsCount
        {
            get => _notificationsCount;
            set => SetProperty(ref _notificationsCount, value);
        }

        private async void LoadAccount()
        {
            LoggedInAccount = await _accountService.GetAccountAsync();
        }

        private async void LoadNotificationsCount()
        {
            NotificationsCount = await _notificationService.GetNotificationsCountAsync();
        }

        #region OnProfileCommand

        private ICommand _onProfileCommand;

        public ICommand OnProfileCommand => _onProfileCommand ??
                                            (_onProfileCommand =
                                                new DelegateCommand(OnProfile));

        private async void OnProfile()
        {
            try
            {
                if (LoggedInAccount != null)
                    await NavigationService.Navigate("ProfileChallengePage");
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnHomeCommand

        private ICommand _onHomeCommand;

        public ICommand OnHomeCommand => _onHomeCommand ?? (_onHomeCommand = new DelegateCommand(OnHome));

        private async void OnHome()
        {
            try
            {
                await NavigationService.Navigate("HomePage");
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnNotificationCommand

        private ICommand _onNotificationCommand;

        public ICommand OnNotificationCommand => _onNotificationCommand ??
                                                 (_onNotificationCommand =
                                                     new DelegateCommand(OnNotification));

        private async void OnNotification()
        {
            try
            {
                await NavigationService.Navigate("NotificationsPage");
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion
    }

    public class HomePageViewModel : MainNavigationViewModel, INavigationAware
    {
        public readonly NewPostPageViewModel NewPostPageViewModel;
        private readonly IAccountService _accountService;
        private readonly IPostService _postService;

        public HomePageViewModel(IUnityContainer container, IPostService postService,
            IAccountService accountService, INotificationService notificationService,
            NewPostPageViewModel newPostPageViewModel) :
            base(container, accountService, notificationService)
        {
            _postService = postService;
            _accountService = accountService;
            NewPostPageViewModel = newPostPageViewModel;
            _postService.PostStatusChangedEventHandler += _postService_PostStatusChangedEventHandler;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                var sync = parameters.GetValue<bool>("Sync");
                if (parameters.GetNavigationMode() == NavigationMode.New || sync)
                {
                    IsPullRefreshLoading = false;
                    Intialize(sync);
                }

                var location = parameters.GetValue<Location>("Location");
                if (location != null)
                    NewPostPageViewModel.NewPost.Location =
                        new PostActivity {Action = "At", Activity = location.Name, Image = location.Image};
            }
            catch (Exception ex)
            {
                ExceptionService.HandleAndShowDialog(ex);
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        private void _postService_PostStatusChangedEventHandler(object sender, PostStatusChangedArgs e)
        {
            try
            {
                if (e.Status == PostStatus.InProgress)
                {
                    ShowProgress();
                    return;
                }

                if (e.Status == PostStatus.HideProgress)
                {
                    HideProgress();
                    return;
                }

                if (e.Post == null || string.IsNullOrEmpty(e.Post.Id))
                    return;

                PostBaseViewModel postVm = PostsViewModels?.FirstOrDefault(a => a.Post.Id == e.Post.Id);

                if (e.Status == PostStatus.Deleted)
                {
                    if (postVm != null)
                    {
                        PostsViewModels.Remove(postVm);
                        HideProgress();
                    }
                }
                else if (e.Status == PostStatus.Added)
                {
                    try
                    {
                        //TODO:Sometimes It Crashed if i removed isrefresh from add post service
                        e.Post.Account = LoggedInAccount;
                        PostBaseViewModel pVm = PostToPostViewModel(e.Post);
                        if (PostsViewModels == null)
                            PostsViewModels = new ObservableCollection<PostBaseViewModel>();
                        PostsViewModels.Insert(0, pVm);
                        RaisePropertyChanged(nameof(PostsViewModels));
                        HideProgress();
                    }
                    catch (Exception ex)
                    {
                        ExceptionService.Handle(ex);
                        OnRefreshPosts();
                    }
                    finally
                    {
                        HideProgress();
                    }
                }
                else if (e.Status == PostStatus.CommentsUpdated)
                {
                    if (postVm != null)
                    {
                        int index = PostsViewModels.IndexOf(postVm);
                        if (index >= 0)
                        {
                            if (postVm.Post != null)
                                postVm.Post.CommentsCount++;
                            PostsViewModels[index] = postVm;
                        }

                        HideProgress();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleAndShowDialog(ex);
                HideProgress();
            }
        }

        #endregion

        #region Properties

        private Account _loggedInAccount;

        public Account LoggedInAccount
        {
            get => _loggedInAccount;
            set
            {
                SetProperty(ref _loggedInAccount, value);
                RaisePropertyChanged(nameof(WelcomeMessage));
            }
        }

        public string WelcomeMessage
        {
            get
            {
                if (!string.IsNullOrEmpty(_loggedInAccount?.FirstName))
                    return string.Format(AppResource.Home_WelcomeMessage_Login, LoggedInAccount.FirstName);
                return AppResource.Home_WelcomeMessage_Logout;
            }
        }

        private bool _hasNotActiveChallenge;

        public bool HasNotActiveChallenge
        {
            get => _hasNotActiveChallenge;
            set => SetProperty(ref _hasNotActiveChallenge, value);
        }

        public bool IsLoggedOut => LoggedInAccount == null;
        public bool IsLoggedIn => LoggedInAccount != null;

        private ObservableCollection<PostBaseViewModel> _postsViewModels;

        public ObservableCollection<PostBaseViewModel> PostsViewModels
        {
            get => _postsViewModels;
            set => SetProperty(ref _postsViewModels, value);
        }

        private bool _isPullRefreshLoading;

        public bool IsPullRefreshLoading
        {
            get => _isPullRefreshLoading;
            set => SetProperty(ref _isPullRefreshLoading, value);
        }

        private bool _noPosts;

        public bool NoPosts
        {
            get => _noPosts;
            set => SetProperty(ref _noPosts, value);
        }

        private int _pageNo;

        #endregion

        #region Methods

        public override async void Intialize(bool sync)
        {
            try
            {
                ShowProgress();
                await LoadAccount(sync);
                await LoadPosts();
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

        private async Task LoadPosts()
        {
            //if (_postService.RefreshPosts)
            //{
            _pageNo = 0;

            List<PostBase> postsList = await _postService.GetPostsAsync(_pageNo, LoggedInAccount?.ChallengeId, _postService.RefreshPosts);
            SetPostViewModel(postsList);
            NoPosts = PostsViewModels == null || PostsViewModels.Count == 0;

            //}
        }

        private void SetPostViewModel(List<PostBase> posts)
        {
            if (posts == null)
                return;


            PostsViewModels = new ObservableCollection<PostBaseViewModel>();
            foreach (PostBase post in posts)
            {
                PostBaseViewModel vm = PostToPostViewModel(post);
                vm.Post = post;
                PostsViewModels.Add(vm);
            }
        }

        private async Task LoadAccount(bool sync)
        {
            LoggedInAccount = await _accountService.GetAccountAsync(sync);
            UpdateChallengeBanner();

            RaisePropertyChanged(nameof(IsLoggedOut));
            RaisePropertyChanged(nameof(IsLoggedIn));
        }

        private void UpdateChallengeBanner()
        {
            HasNotActiveChallenge = true; //LoggedInAccount != null && LoggedInAccount.HasNotActiveChallenge;
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

        #region LoginCommand

        private ICommand _loginCommand;

        public ICommand OnLoginCommand => _loginCommand ??
                                          (_loginCommand =
                                              new DelegateCommand(Login));

        private async void Login()
        {
            try
            {
                await NavigationService.Navigate("LoginPage");
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

        #endregion

        #region OnNewPostCommand

        private ICommand _onNewPostCommand;

        public ICommand OnNewPostCommand => _onNewPostCommand ??
                                            (_onNewPostCommand =
                                                new DelegateCommand(OnNewPost));

        private async void OnNewPost()
        {
            try
            {
                bool isLogginIn = await _accountService.LoginFirstAsync();
                if (isLogginIn)
                    await NavigationService.Navigate("NewPostPage");
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
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

        #region OnSearchFriendsCommand

        private ICommand _onSearchFriendsCommand;

        public ICommand OnSearchFriendsCommand => _onSearchFriendsCommand ??
                                                  (_onSearchFriendsCommand =
                                                      new DelegateCommand(OnSearchFriends));

        private async void OnSearchFriends()
        {
            try
            {
                bool isLogginIn = await _accountService.LoginFirstAsync();
                if (isLogginIn)
                    await NavigationService.Navigate("SearchFriendPage");
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnGetMorePostsCommand

        private ICommand _onGetMorePostsCommand;

        public ICommand OnGetMorePostsCommand => _onGetMorePostsCommand ??
                                                 (_onGetMorePostsCommand =
                                                     new DelegateCommand(OnGetMorePosts));

        private async void OnGetMorePosts()
        {
            try
            {
                // ShowProgress();
                _pageNo++;
                List<PostBase> nextPageItems = await _postService.GetPostsAsync(_pageNo, LoggedInAccount?.ChallengeId);
                if (nextPageItems != null && nextPageItems.Count > 0)
                    foreach (PostBase item in nextPageItems)
                        PostsViewModels.Add(PostToPostViewModel(item));
                else
                    _pageNo--;
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        private PostBaseViewModel PostToPostViewModel(PostBase post)
        {
            try
            {
                PostBaseViewModel vm;
                if (post is Post)
                    vm = new PostBaseViewModel(Container);
                //else if (post is PostCampaign)
                //    vm = new PostCampaignViewModel();
                //else if (post is PostPeopleYouKnow)
                //    vm = new PostPeopleYouMayKnowViewModel();
                else if (post is PostAd)
                    vm = new PostAddViewModel(Container);
                //else if (post is PostWeekly)
                //    vm = new PostWeeklyViewModel();
                else
                    vm = new PostBaseViewModel(Container);

                vm.Post = post;
                return vm;
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
                return null;
            }
        }

        #endregion

        #region OnRefreshPostsCommand

        private ICommand _onRefreshPostsCommand;

        public ICommand OnRefreshPostsCommand => _onRefreshPostsCommand ??
                                                 (_onRefreshPostsCommand =
                                                     new DelegateCommand(OnRefreshPosts));

        private void OnRefreshPosts()
        {
            try
            {
                IsPullRefreshLoading = true;
                //postService.RefreshPosts = true;
                Intialize(false);
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