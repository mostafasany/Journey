using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Abstractions.Forms;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Journey.Models.Account;
using Journey.Models.Post;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Post;
using Prism.Commands;
using Prism.Navigation;
using Unity;
using Journey.Services.Buisness.ChallengeActivity;
using Journey.Services.Buisness.Challenge;

namespace Journey.ViewModels
{
    public class NewPostPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IBlobService _blobService;
        private readonly IMediaService<Media> _mediaService;
        private readonly IPostService _postService;
        private readonly ISettingsService _settingsService;
        private const string LastPostDate = "LastPostDate";
        private readonly IChallengeActivityService _challengeActivityService;
        private readonly IChallengeService _challengeService;
        public NewPostPageViewModel(IUnityContainer container, IBlobService blobService,
            IPostService postService, IMediaService<Media> mediaService,
                                    IAccountService accountService, ISettingsService settingsService,
                                    IChallengeActivityService challengeActivityService,
                                    IChallengeService challengeService) :
            base(container)
        {
            _challengeService = challengeService;
            _challengeActivityService = challengeActivityService;
            _mediaService = mediaService;
            _postService = postService;
            _blobService = blobService;
            _accountService = accountService;
            _settingsService = settingsService;
            NewPost = new Post();
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                if (parameters?.GetNavigationMode() == NavigationMode.Back)
                {
                    _location = parameters.GetValue<Location>("Location");
                    if (_location != null)
                        NewPost.Location =
                                   new PostActivity { Action = "At", Activity = _location.Name, Image = _location.Image };
                }

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

        private Location _location;
        Models.Challenge.Challenge _challenge;

        private Account _loggedInAccount;

        public Account LoggedInAccount
        {
            get => _loggedInAccount;
            set => SetProperty(ref _loggedInAccount, value);
        }

        private List<string> _imagesPath = new List<string>();

        private Post _newPost;

        public Post NewPost
        {
            get => _newPost;
            set => SetProperty(ref _newPost, value);
        }

        private bool _addPostToChallenge;

        public bool AddPostToChallenge
        {
            get => _addPostToChallenge;
            set => SetProperty(ref _addPostToChallenge, value);
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

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                LoggedInAccount = await _accountService.GetAccountAsync();
                _challenge = await _challengeService.GetChallengeAsync(_accountService.LoggedInAccount.ChallengeId);

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
                //if (NewPost?.HasLocation == false)
                //{
                //    await DialogService.ShowMessageAsync(AppResource.Post_LocationMust, AppResource.Error);
                //    return;
                //}

                //if (NewPost?.MediaList == null || NewPost?.MediaList?.Count == 0)
                //{
                //    await DialogService.ShowMessageAsync(AppResource.Post_UploadImageMust, AppResource.Error);
                //    return;
                //}

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
        }

        private async void Post()
        {
            ShowProgress();
            if (_imagesPath.Count == 0 && NewPost.MediaList != null)
                foreach (Media image in NewPost.MediaList)
                {
                    string path = await _blobService.UploadAsync(image.SourceArray, image.Name);
                    if (!string.IsNullOrEmpty(path))
                        _imagesPath.Add(path);
                }

            ShowProgress();
            if (!string.IsNullOrEmpty(LoggedInAccount.ChallengeId))
                NewPost.Challenge = LoggedInAccount.ChallengeId;
            Post post = await _postService.AddPostAsync(NewPost, _imagesPath);
            if (post == null)
            {
                await DialogService.ShowMessageAsync(AppResource.NewPost_NewPostError, AppResource.Error);
                return;
            }

            if (!string.IsNullOrEmpty(_accountService.LoggedInAccount.ChallengeId) &&
                _challenge.SelectedLocation?.Id == _location?.Id)
            {
                string date = await _settingsService.Get(LastPostDate);
                DateTime.TryParse(date, out DateTime parsedDate);
                if (parsedDate.Date != DateTime.Now.Date)
                {
                    await _settingsService.Set(LastPostDate, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture));

                    await _challengeActivityService.AddActivityAsync(new Models.Challenge.ChallengeWorkoutActivityLog()
                    {
                        Account = _accountService.LoggedInAccount,
                        Challenge = _challenge.Id,
                        DatetTime = DateTime.Now,
                        Location = _location,
                    });
                }
            }

            NewPost = new Post();

            _imagesPath = new List<string>();
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
                if (IsProgress())
                    return;
                var commands =
                    new List<DialogCommand>
                    {
                        new DialogCommand
                        {
                            Label = AppResource.Camera,
                            Invoked = async () =>
                            {
                                try
                                {
                                    Media media = await _mediaService.TakePhotoAsync();
                                    AddMedia(media);
                                }
                                catch (NotSupportedException)
                                {
                                    await DialogService.ShowMessageAsync(AppResource.Camera_NotSupported, AppResource.Error);
                                }
                            }
                        },
                        new DialogCommand
                        {
                            Label = AppResource.Gallery,
                            Invoked = async () =>
                            {
                                try
                                {
                                    Media media = await _mediaService.PickPhotoAsync();
                                    AddMedia(media);
                                }
                                catch (NotSupportedException)
                                {
                                    await DialogService.ShowMessageAsync(AppResource.Camera_NotSupported, AppResource.Error);
                                }
                            }
                        },
                        new DialogCommand
                        {
                            Label = AppResource.TakeVideo,
                            Invoked = async () =>
                            {
                                try
                                {
                                    Media media = await _mediaService.TakeVideoAsync();
                                    AddMedia(media);
                                }
                                catch (NotSupportedException)
                                {
                                    await DialogService.ShowMessageAsync(AppResource.Camera_NotSupported, AppResource.Error);
                                }
                            }
                        },
                        new DialogCommand
                        {
                            Label = AppResource.PickVideo,
                            Invoked = async () =>
                            {
                                try
                                {
                                    Media media = await _mediaService.PickVideoAsync();
                                    AddMedia(media);
                                }
                                catch (NotSupportedException)
                                {
                                    await DialogService.ShowMessageAsync(AppResource.Camera_NotSupported, AppResource.Error);
                                }
                            }
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
            if (media == null)
                return;

            if (NewPost.MediaList == null)
                NewPost.MediaList = new ObservableCollection<Media>();

            List<Media> list = NewPost.MediaList.ToList();
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
            if (IsProgress())
                return;
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

        private void OnImageDelete(Media media)
        {
            try
            {
                if (media != null)
                {
                    NewPost.MediaList.Remove(media);
                    ObservableCollection<Media> temp = NewPost.MediaList;
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

        private void OnClose()
        {
            NavigationService.GoBack();
        }

        #endregion

        #endregion
    }
}