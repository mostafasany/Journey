using System;
using System.Windows.Input;
using Journey.Services.Buisness.Account;
using Prism.Commands;
using Prism.Navigation;
using Tawasol.Models;
using Unity;

namespace Journey.ViewModels
{
    public class UpdateProfilePageViewModel : BaseViewModel, INavigationAware
    {
        public UpdateProfilePageViewModel(IUnityContainer container, IAccountService accountService) :
            base(container)
        {
            _accountService = accountService;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public  async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
               ShowProgress();
                LoggedInAccount = await _accountService.GetAccountAsync();
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

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        #endregion

        #region Properties

        private Account loggedInAccount;

        public Account LoggedInAccount
        {
            get => loggedInAccount;
            set => SetProperty(ref loggedInAccount, value);
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
        private readonly IAccountService _accountService;

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