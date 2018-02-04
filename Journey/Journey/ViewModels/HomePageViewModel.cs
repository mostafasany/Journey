using System;
using System.Windows.Input;
using Journey.Views;
using Prism.Commands;
using Prism.Navigation;
using Unity;
using Xamarin.Forms;

namespace Journey.ViewModels
{
    public class HomePageViewModel : BaseViewModel, INavigationAware
    {
        public HomePageViewModel(IUnityContainer container) :
            base(container)
        {
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }

        #endregion

        #region Properties

        private string _text = "Hello Mostafa";

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

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

        #region LoginCommand

        private ICommand _loginCommand;

        public ICommand LoginCommand => _loginCommand ??
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

        #endregion
    }
}