using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Abstractions.Services.Contracts;
using Journey.Models.Post;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Post;
using Prism.Commands;
using Unity;

namespace Journey.ViewModels.Wall
{
    public class PostBaseViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IPostService _postService;
        private readonly IShareService _shareService;

        public PostBaseViewModel(IUnityContainer container) :
            base(container)
        {
            _postService = container.Resolve<IPostService>();
            _shareService = container.Resolve<IShareService>();
            _accountService = container.Resolve<IAccountService>();
        }

        #region Propeties

        private PostBase _post;

        public PostBase Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        #endregion

        #region Commands

        #region OnPostDeleteCommand

        private ICommand _onPostDeleteCommand;

        public ICommand OnPostDeleteCommand => _onPostDeleteCommand ??
                                               (_onPostDeleteCommand =
                                                   new DelegateCommand(OnPostDelete));

        private async void OnPostDelete()
        {
            try
            {
                if (_post == null)
                    return;
                var isLogginIn = await _accountService.LoginFirstAsync();
                if (isLogginIn)
                {
                    var deleteCommand = new DialogCommand
                    {
                        Label = "Delete",
                        Invoked = async () =>
                        {
                            var status = await _postService.DeletePostAsync(_post);
                            if (!status)
                                await DialogService.ShowMessageAsync("Error", "Error while deleting your post");
                        }
                    };

                    var reportCommand = new DialogCommand
                    {
                        Label = "Report",
                        Invoked = async () =>
                        {
                            await DialogService.ShowMessageAsync("Sent", "Your report has been sent");
                        }
                    };

                    var cancelCommand = new DialogCommand
                    {
                        Label = "Cancel"
                    };

                    var loggedInAccount = await _accountService.GetAccountAsync();
                    var commands = new List<DialogCommand>
                    {
                        _post?.Account?.Id != loggedInAccount?.Id ? reportCommand : deleteCommand,
                        cancelCommand
                    };

                    await DialogService.ShowMessageAsync("", "What action you need", commands);
                }
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnPostShareCommand

        private ICommand _onPostShareCommand;

        public ICommand OnPostShareCommand => _onPostShareCommand ??
                                              (_onPostShareCommand =
                                                  new DelegateCommand(OnPostShare));

        private async void OnPostShare()
        {
            try
            {
                if (_post == null)
                    return;

                var isLogginIn = await _accountService.LoginFirstAsync();
                if (isLogginIn)
                {
                    _shareService.Share(_post.Feed, "Mostafa", _post?.MediaList?.FirstOrDefault()?.Path);
                    await _postService.ShareAsync(_post);
                    _post.SharesCount++;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnPostLikeCommand

        private ICommand _onPostLikeCommand;

        public ICommand OnPostLikeCommand => _onPostLikeCommand ??
                                             (_onPostLikeCommand =
                                                 new DelegateCommand(OnPostLike));

        private async void OnPostLike()
        {
            try
            {
                if (_post == null)
                    return;
                var isLogginIn = await _accountService.LoginFirstAsync();
                if (isLogginIn)
                {
                    _post.Liked = !_post.Liked;
                    if (_post.Liked)
                        _post.LikesCount++;
                    else
                        _post.LikesCount--;

                    await _postService.LikeAsync(_post);
                }
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnPostCommentCommand

        private ICommand _onPostCommentCommand;

        public ICommand OnPostCommentCommand => _onPostCommentCommand ??
                                                (_onPostCommentCommand =
                                                    new DelegateCommand(OnPostComment));

        private async void OnPostComment()
        {
            try
            {
                var isLogginIn = await _accountService.LoginFirstAsync();
                if (isLogginIn)
                    await NavigationService.Navigate("NewCommentPage", _post.Id, "Post");
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnProfileDetailsCommand

        private ICommand _onProfileDetailsCommand;

        public ICommand OnProfileDetailsCommand => _onProfileDetailsCommand ??
                                                   (_onProfileDetailsCommand =
                                                       new DelegateCommand(OnProfileDetails));

        private async void OnProfileDetails()
        {
            try
            {
                if (_post == null)
                    return;
                var isLogginIn = await _accountService.LoginFirstAsync();
                if (isLogginIn)
                {
                    var account = _post.Account;
                    if (account != null)
                    {
                        var loggedInAccount = await _accountService.GetAccountAsync();
                        if (account.Id == loggedInAccount.Id)
                            await NavigationService.Navigate("ProfilePage");
                        else
                            await NavigationService.Navigate("FriendsPage", account.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #region OnGalleryDetailsCommand

        private ICommand _onGalleryDetailsCommand;

        public ICommand OnGalleryDetailsCommand => _onGalleryDetailsCommand ??
                                                   (_onGalleryDetailsCommand =
                                                       new DelegateCommand(OnGalleryDetails));

        private async void OnGalleryDetails()
        {
            try
            {
                if (_post.MediaList.Any())
                    await NavigationService.Navigate("MediaPage", _post.MediaList, "Media");
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #endregion
    }
}