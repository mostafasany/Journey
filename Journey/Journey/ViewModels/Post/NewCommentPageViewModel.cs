using System;
using System.Collections.Generic;
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

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                _postId = parameters.GetValue<string>("Post") ?? "";
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

        private ObservableCollection<Comment> _comments;

        public ObservableCollection<Comment> Comments
        {
            get => _comments;
            set
            {
                _comments = value;
                RaisePropertyChanged();
            }
        }

        public bool NoComments => Comments == null || Comments.Count == 0;

        private string _newComment;

        public string NewComment
        {
            get => _newComment;
            set
            {
                _newComment = value;
                RaisePropertyChanged();
            }
        }

        private bool _isPullRefreshLoading;

        public bool IsPullRefreshLoading
        {
            get => _isPullRefreshLoading;
            set => SetProperty(ref _isPullRefreshLoading, value);
        }

        private string _postId;

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                base.Intialize(sync);
                LoggedInAccount = await _accountService.GetAccountAsync();
                Comments = new ObservableCollection<Comment>();
                List<Comment> commentsDTo = await _postCommentService.GetCommentsAsync(_postId, true);
                if (commentsDTo != null)
                    Comments = new ObservableCollection<Comment>(commentsDTo);

                RaisePropertyChanged(nameof(NoComments));
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
                    Comment comment = await _postCommentService.AddCommentAsync(NewComment, _postId);
                    if (comment != null)
                    {
                        comment.Account = LoggedInAccount;
                        comment.Mine = true;
                        Comments.Insert(0, comment);
                        NewComment = "";
                        RaisePropertyChanged(nameof(NoComments));
                    }
                }
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

        #region OnCloseCommand

        public DelegateCommand OnCloseCommand => new DelegateCommand(OnClose);

        private void OnClose()
        {
            NavigationService.GoBack();
        }

        #endregion

        #region OnPullRefreshRequestCommand

        public DelegateCommand OnPullRefreshRequestCommand => new DelegateCommand(OnPullRefreshRequest);

        private void OnPullRefreshRequest()
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