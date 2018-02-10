using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Journey.Models;
using Journey.Models.Account;
using Journey.Models.Post;
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
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                ShowProgress();
                IsPullRefreshLoading = false;
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
                    return LoggedInAccount.FirstName + ", Want to share an update";
                return "Login to share an update";
            }
        }

        private bool _hasNotActiveChallenge;

        public bool HasNotActiveChallenge
        {
            get => _hasNotActiveChallenge;
            set => SetProperty(ref _hasNotActiveChallenge, value);
        }

        public bool IsLoggedOut => LoggedInAccount == null;

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

        private int _pageNo;

        #endregion

        #region Methods

        public override async void Intialize()
        {
            try
            {
                ShowProgress();
                if (_accountService.IsAccountAuthenticated())
                {
                    var tasks = new List<Task>
                    {
                        LoadAccount(false),
                        LoadPosts()
                    };
                    ShowProgress();
                    await Task.WhenAll(tasks);
                }
                else
                {
                    await LoadAccount(false);
                    await LoadPosts();
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

        private async Task LoadPosts()
        {
            if (_postService.RefreshPosts)
            {
                _pageNo = 0;

                var postsList = await _postService.GetPostsAsync(_pageNo, null, _postService.RefreshPosts);
                SetPostViewModel(postsList);
            }
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
            if (sync)
            {
                await _accountService.GetAccountAsync(true);
            }
            else
            {
                LoggedInAccount = await _accountService.GetAccountAsync();
                UpdateChallengeBanner();
            }

            RaisePropertyChanged(nameof(IsLoggedOut));
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
                    await NavigationService.Navigate("InviteFriendPage");
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

        #region OnGetMorePostsCommand

        private ICommand _onGetMorePostsCommand;

        public ICommand OnGetMorePostsCommand => _onGetMorePostsCommand ??
                                                 (_onGetMorePostsCommand =
                                                     new DelegateCommand(OnGetMorePosts));

        private async void OnGetMorePosts()
        {
            try
            {
                ShowProgress();
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
                HideProgress();
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
                Intialize();
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