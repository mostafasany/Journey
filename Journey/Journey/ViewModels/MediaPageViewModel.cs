using System;
using System.Collections.Generic;
using Journey.Models;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.PostComment;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class MediaPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IPostCommentService _postCommentService;

        public MediaPageViewModel(IUnityContainer container,
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
                if (parameters.GetNavigationMode() == NavigationMode.New)
                    MediaList = parameters.GetValue<IEnumerable<Media>>("Media") ?? null;
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

        private IEnumerable<Media> mediaList;

        public IEnumerable<Media> MediaList
        {
            get => mediaList;
            set
            {
                mediaList = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        public override void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
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

        #region OnGalleryDetailsCommand

        private DelegateCommand<Media> _onGalleryDetailsCommand;

        public DelegateCommand<Media> OnGalleryDetailsCommand => _onGalleryDetailsCommand ??
                                                                 (_onGalleryDetailsCommand =
                                                                     new DelegateCommand<Media>(OnGalleryDetails));

        private async void OnGalleryDetails(Media media)
        {
            try
            {
                if (media.Type == MediaType.Video)
                    await NavigationService.Navigate("VideoPage", media, "Media");
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