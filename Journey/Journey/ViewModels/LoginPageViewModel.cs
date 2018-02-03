using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class LoginPageViewModel : BaseViewModel, INavigationAware
    {
        public LoginPageViewModel(IUnityContainer container) :
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

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            var d = parameters["IsLogin"];
        }

        //public override void OnNavigatedTo(object paramater, bool isBack)
        //{
        //    base.OnNavigatedTo(paramater, isBack);
        //    Intialize();
        //}

        //public override void OnNavigatingFrom()
        //{
        //    base.OnNavigatingFrom();
        //}

        //public override void OnNavigatedFrom()
        //{
        //    base.OnNavigatedFrom();
        //}

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
                if (App.Authenticator != null)
                {
                    var authenticated = await App.Authenticator.Authenticate();
                }
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