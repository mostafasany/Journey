﻿using System;
using System.Windows.Input;
using Journey.Resources;
using Journey.Services.Azure;
using Journey.Services.Buisness.Account;
using Microsoft.WindowsAzure.MobileServices;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class LoginPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IAzureService _azureService;

        public LoginPageViewModel(IUnityContainer container, IAzureService azureService) :
            base(container)
        {
            //TODO:THis shouldnt exits it should be injected but sometimes account service resolved twice cause override eachother
            _accountService = container.Resolve<IAccountService>();
            _azureService = azureService;
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

        #endregion

        #region Properties

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

        #region LoginCommand

        private ICommand _loginCommand;

        public ICommand LoginCommand => _loginCommand ??
                                        (_loginCommand =
                                            new DelegateCommand(Login));

        private async void Login()
        {
            try
            {
                if (App.Authenticator == null) return;

                MobileServiceUser authenticated = await _accountService.AutehticateAsync();
                if (authenticated == null)
                {
                    await DialogService.ShowMessageAsync(AppResource.Login_CantLoginMessage,
                        AppResource.Login_CantLoginTitle);
                    return;
                }

                ShowProgress();


                MobileServiceClient client = _azureService.CreateOrGetAzureClient(authenticated.UserId,
                    authenticated.MobileServiceAuthenticationToken);

                bool isLoggedIn = await _accountService.SoicalLoginAndSaveAsync(client);
                if (isLoggedIn)
                    await NavigationService.Navigate("UpdateProfilePage", _accountService.LoggedInAccount, "Account");
                else
                    await DialogService.ShowMessageAsync(AppResource.Login_CantLoginMessage,
                        AppResource.Login_CantLoginTitle);
            }
            catch (Exception e)
            {
                ExceptionService.HandleAndShowDialog(e, AppResource.Login_CantLoginMessage);
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