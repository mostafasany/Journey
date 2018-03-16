using System;
using Abstractions.Forms;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ImagePageViewModel : BaseViewModel, INavigationAware
    {
        public ImagePageViewModel(IUnityContainer container) :
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
                Media = parameters.GetValue<Media>("Media");
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

        private Media _media;

        public Media Media
        {
            get => _media;
            set
            {
                _media = value;
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