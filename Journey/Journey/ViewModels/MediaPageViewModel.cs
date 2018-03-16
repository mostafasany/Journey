using System;
using System.Collections.Generic;
using Abstractions.Forms;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class MediaPageViewModel : BaseViewModel, INavigationAware
    {
        public MediaPageViewModel(IUnityContainer container) :
            base(container)
        {
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                if (parameters.GetNavigationMode() == NavigationMode.New)
                    MediaList = parameters.GetValue<IEnumerable<Media>>("Media");
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

        private IEnumerable<Media> _mediaList;

        public IEnumerable<Media> MediaList
        {
            get => _mediaList;
            set
            {
                _mediaList = value;
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
                else
                    await NavigationService.Navigate("ImagePage", media, "Media");
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