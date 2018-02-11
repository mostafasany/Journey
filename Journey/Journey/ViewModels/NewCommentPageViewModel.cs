using System;
using System.Collections.ObjectModel;
using Journey.Models.Account;
using Journey.Models.Post;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.PostComment;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class NewCommentPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IPostCommentService _postCommentService;

        public NewCommentPageViewModel(IUnityContainer container,
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
                PostId = parameters.GetValue<string>("Post") ?? "";
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

        private Account _loggedInAccount;

        public Account LoggedInAccount
        {
            get => _loggedInAccount;
            set => SetProperty(ref _loggedInAccount, value);
        }

        private ObservableCollection<Comment> comments;

        public ObservableCollection<Comment> Comments
        {
            get => comments;
            set
            {
                comments = value;
                RaisePropertyChanged();
            }
        }


        private string newComment;

        public string NewComment
        {
            get => newComment;
            set
            {
                newComment = value;
                RaisePropertyChanged();
            }
        }

        private bool isPullRefreshLoading;

        public bool IsPullRefreshLoading
        {
            get => isPullRefreshLoading;
            set => SetProperty(ref isPullRefreshLoading, value);
        }

        private string PostId;

        #endregion

        #region Methods

        public override async void Intialize()
        {
            try
            {
                ShowProgress();
                base.Intialize();
                LoggedInAccount = await _accountService.GetAccountAsync();
                Comments = new System.Collections.ObjectModel.ObservableCollection<Comment>();

                var postDTo = await _postCommentService.GetCommentsAsync(PostId, true);
                if (postDTo != null)
                    Comments = new System.Collections.ObjectModel.ObservableCollection<Comment>(postDTo);
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
                if (!string.IsNullOrEmpty(NewComment))
                {
                    var comment = await _postCommentService.AddCommentAsync(NewComment, PostId);
                    if (comment != null)
                    {
                        comment.Account = LoggedInAccount;
                        Comments.Add(comment);
                        NewComment = "";
                    }
                }
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

        #region OnCloseCommand

        public DelegateCommand OnCloseCommand => new DelegateCommand(OnClose);

        private async void OnClose()
        {
            NavigationService.GoBack();
        }

        #endregion

        #region OnPullRefreshRequestCommand

        public DelegateCommand OnPullRefreshRequestCommand => new DelegateCommand(OnPullRefreshRequest);

        private async void OnPullRefreshRequest()
        {
            try
            {
                IsPullRefreshLoading = true;

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