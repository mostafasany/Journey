using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Journey.Constants;
using Journey.Models;
using Journey.Models.Account;
using Journey.Models.Post;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Post;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class NewPostPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IBlobService _blobService;
        private readonly IMediaService<Media> _mediaService;
        private readonly IPostService _postService;

        public NewPostPageViewModel(IUnityContainer container, IBlobService blobService,
            IPostService postService, IMediaService<Media> mediaService, IAccountService accountService) :
            base(container)
        {
            _mediaService = mediaService;
            _postService = postService;
            _blobService = blobService;
            _accountService = accountService;
            NewPost = new Post();
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                if (parameters.GetNavigationMode() == NavigationMode.Back)
                {
                    var location = parameters.GetValue<Location>("Location");
                    if (location != null)
                        NewPost.Location = new PostActivity { Action = "At", Activity = location.Name, Image = location.Image };

                }

                LoggedInAccount = await _accountService.GetAccountAsync();
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

        private List<string> imagesPath = new List<string>();

        private Post newPost;

        public Post NewPost
        {
            get => newPost;
            set => SetProperty(ref newPost, value);
        }

        private bool addPostToChallenge = false;

        public bool AddPostToChallenge
        {
            get => addPostToChallenge;
            set => SetProperty(ref addPostToChallenge, value);
        }

        //private ObservableCollection<Activity> activityList;
        //public ObservableCollection<Activity> ActivityList
        //{
        //    get => activityList;
        //    set => Set(ref activityList, value);
        //}

        //private Activity selectedActivity;
        //public Activity SelectedActivity
        //{
        //    get => selectedActivity;
        //    set
        //    {
        //        Set(ref selectedActivity, value);
        //        SelectedSubActivity = SelectedActivity?.ActivityList?.FirstOrDefault();
        //    }

        //}

        //private Activity selectedSubActivity;
        //public Activity SelectedSubActivity
        //{
        //    get => selectedSubActivity;
        //    set
        //    {
        //        if (value == null)
        //            return;
        //        Set(ref selectedSubActivity, value);

        //        ActivityChanged(value);
        //    }
        //}

        #endregion

        #region Methods

        public override void Intialize()
        {
            try
            {
                ShowProgress();
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

        #region OnNewPostCommand

        private ICommand _onNewPostCommand;

        public ICommand OnNewPostCommand => _onNewPostCommand ??
                                            (_onNewPostCommand =
                                                new DelegateCommand(OnNewPost));

        private async void OnNewPost()
        {
            try
            {
                if (IsProgress())
                    return;

                var commands =
                    new List<DialogCommand>
                    {
                        new DialogCommand
                        {
                           Label = AppResource.Yes,
                            Invoked = Post
                        },
                        new DialogCommand
                        {
                            Label = AppResource.Cancel
                        }
                    };

                await DialogService.ShowMessageAsync("", AppResource.NewPost_NewPostMessage,
                    commands);
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

        private async void Post()
        {
            ShowProgress();
            if (imagesPath.Count == 0 && NewPost.MediaList != null)
                foreach (var image in NewPost.MediaList)
                {
                    var id = Guid.NewGuid();
                    var ex = image.Ext;
                    if (string.IsNullOrEmpty(ex))
                        ex = Constant.DefaultImageExt;
                    var fileName = string.Format("{0}.{1}", id, ex);
                    var path = await _blobService.UploadAsync(image.SourceArray, fileName);
                    if (!string.IsNullOrEmpty(path))
                        imagesPath.Add(path);
                }
            ShowProgress();
            if (AddPostToChallenge)
                NewPost.Challenge = LoggedInAccount.ChallengeId;
            var post = await _postService.AddPostAsync(NewPost, imagesPath);
            if (post == null)
            {
                await DialogService.ShowMessageAsync(AppResource.Error, AppResource.NewPost_NewPostError);
                return;
            }
            // SelectedActivity = null;
            NewPost = new Post();

            imagesPath = new List<string>();
            NavigationService.GoBack();
        }

        #endregion

        #region OnAddPhotoOrVideoCommand

        private ICommand _onAddPhotoOrVideoCommand;

        public ICommand OnAddPhotoOrVideoCommand => _onAddPhotoOrVideoCommand ??
                                                    (_onAddPhotoOrVideoCommand =
                                                        new DelegateCommand(OnAddPhotoOrVideo));

        private async void OnAddPhotoOrVideo()
        {
            try
            {
                var commands =
                    new List<DialogCommand>
                    {
                        new DialogCommand
                        {
                            Label = AppResource.Camera,
                        Invoked = async () => { AddMedia(await _mediaService.TakePhotoAsync()); }
                        },
                        new DialogCommand
                        {
                        Label = AppResource.Video,
                        Invoked = async () => { AddMedia(await _mediaService.TakeVideoAsync()); }
                        },
                        new DialogCommand
                        {
                            Label = AppResource.Cancel
                        }
                    };

                await DialogService.ShowMessageAsync("", AppResource.NewPost_UploadMedia,
                    commands);
            }
            catch (Exception ex)
            {
                ExceptionService.HandleAndShowDialog(ex);
            }
        }

        private void AddMedia(Media media)
        {
            if (NewPost.MediaList == null)
                NewPost.MediaList = new ObservableCollection<Media>();

            var list = NewPost.MediaList.ToList();
            list.Add(media);
            NewPost.MediaList = new ObservableCollection<Media>(list);

        }

        #endregion

        #region OnCheckInCommand

        private ICommand _onCheckInCommand;

        public ICommand OnCheckInCommand => _onCheckInCommand ??
                                            (_onCheckInCommand =
                                                new DelegateCommand(OnCheckIn));

        private async void OnCheckIn()
        {
            await NavigationService.Navigate("ChooseLocationPage");
        }

        #endregion

        #region OnGalleryDetailsCommand

        private DelegateCommand _onGalleryDetailsCommand;

        public DelegateCommand OnGalleryDetailsCommand => _onGalleryDetailsCommand ??
                                                                  (_onGalleryDetailsCommand =
                                                                      new DelegateCommand(OnGalleryDetails));

        private async void OnGalleryDetails()
        {
            try
            {
                await NavigationService.Navigate("MediaPage", NewPost.MediaList, "Media");
            }
            catch (Exception ex)
            {
                ExceptionService.HandleAndShowDialog(ex);
            }
        }

        #endregion

        #region OnImageDeleteCommand

        public DelegateCommand<Media> OnImageDeleteCommand => new DelegateCommand<Media>(OnImageDelete);

        private async void OnImageDelete(Media media)
        {
            try
            {
                if (media != null)
                {
                    NewPost.MediaList.Remove(media);
                    var temp = NewPost.MediaList;
                    NewPost.MediaList = new ObservableCollection<Media>(temp);
                }
            }
            catch (Exception ex)
            {
                ExceptionService.HandleAndShowDialog(ex);
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

        #endregion
    }
}