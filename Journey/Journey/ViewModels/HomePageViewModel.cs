using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Abstractions.Forms;
using Journey.Models.Account;
using Journey.Models.Post;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Post;
using Journey.ViewModels.Wall;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class HomePageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IPostService _postService;

        public HomePageViewModel(IUnityContainer container, IPostService postService, IAccountService accountService) :
            base(container)
        {
            _postService = postService;
            _accountService = accountService;
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

                var postVm = PostsViewModels?.FirstOrDefault(a => a.Post.Id == e.Post.Id);

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
                        var pVm = PostToPostViewModel(e.Post);
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
                        var index = PostsViewModels.IndexOf(postVm);
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

        public Media Image => LoggedInAccount == null
            ? new Media {Path = "http://bit.ly/2zBffZy"}
            : _loggedInAccount.Image;

        private Account _loggedInAccount;

        public Account LoggedInAccount
        {
            get => _loggedInAccount;
            set
            {
                SetProperty(ref _loggedInAccount, value);
                RaisePropertyChanged(nameof(Image));
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
                if (_accountService.IsAccountAuthenticated())
                {
                    var tasks = new List<Task>
                    {
                        LoadAccount(sync),
                        LoadPosts()
                    };
                    ShowProgress();
                    await Task.WhenAll(tasks);
                }
                else
                {
                    await LoadAccount(sync);
                    await LoadPosts();
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

        private async Task LoadPosts()
        {
            //if (_postService.RefreshPosts)
            //{
            _pageNo = 0;

            var postsList = await _postService.GetPostsAsync(_pageNo, null, _postService.RefreshPosts);
            SetPostViewModel(postsList);
            NoPosts = PostsViewModels == null || PostsViewModels.Count == 0;

            //}
        }

        private void SetPostViewModel(List<PostBase> posts)
        {
            if (posts == null)
                return;


            PostsViewModels = new ObservableCollection<PostBaseViewModel>();
            foreach (var post in posts)
            {
                var vm = PostToPostViewModel(post);
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
            HasNotActiveChallenge = LoggedInAccount != null && LoggedInAccount.HasNotActiveChallenge;
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
                var isLogginIn = await _accountService.LoginFirstAsync();
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
                var isLogginIn = await _accountService.LoginFirstAsync();
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
                var isLogginIn = await _accountService.LoginFirstAsync();
                if (isLogginIn)
                    await NavigationService.Navigate("SearchFriendPage");
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnProfileCommand

        private ICommand _onProfileCommand;

        public ICommand OnProfileCommand => _onProfileCommand ??
                                            (_onProfileCommand =
                                                new DelegateCommand(OnProfile));

        private async void OnProfile()
        {
            try
            {
                await NavigationService.Navigate("ProfileChallengePage");
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
                var nextPageItems = await _postService.GetPostsAsync(_pageNo);
                if (nextPageItems != null && nextPageItems.Count > 0)
                    foreach (var item in nextPageItems)
                        PostsViewModels.Add(PostToPostViewModel(item));
                else
                    _pageNo--;
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
            finally
            {
                //HideProgress();
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