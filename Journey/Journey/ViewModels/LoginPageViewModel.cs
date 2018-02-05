using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Abstractions.Contracts;
using Abstractions.Models;
using Journey.Services.Azure;
using Journey.Services.Buisness.Account;
using Microsoft.WindowsAzure.MobileServices;
using Prism.Commands;
using Prism.Navigation;
using Tawasol.Models;
using Unity;

namespace Journey.ViewModels
{
    public class LoginPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IAzureService _azureService;

        public LoginPageViewModel(IUnityContainer container, IAzureService azureService,
            AccountService accountService) :
            base(container)
        {
            _accountService = accountService;
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

        //private string _text;

        //public string Text
        //{
        //    get => _text;
        //    set => SetProperty(ref _text, value);
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

        private async void FillUserData(MobileServiceClient client)
        {
            try
            {
                var socialInfo = await client.InvokeApiAsync<List<Social>>("/.auth/me");
                var info = socialInfo.FirstOrDefault();
                _accountService.Token = info.AccessToken;
                if (string.IsNullOrEmpty(_accountService.Token))
                {
                    return;
                }
                var account = await _accountService.GetAccountAsync();
                if (account == null)
                {
                    return;
                }
                var loggedInAccount = new Account
                {
                    FirstName = string.IsNullOrEmpty(account.FirstName)
                        ? info.Claims?.Where(a => a.Typ.Contains("givenname")).FirstOrDefault()?.Val
                        : account.FirstName,
                    LastName = string.IsNullOrEmpty(account.LastName)
                        ? info.Claims?.Where(a => a.Typ.Contains("surname")).FirstOrDefault()?.Val
                        : account.LastName,
                    Email = info.Claims?.Where(a => a.Typ.Contains("emailaddress")).FirstOrDefault()?.Val,
                    Gender = info.Claims?.Where(a => a.Typ.Contains("gender")).FirstOrDefault()?.Val,
                    SID = info.Claims?.Where(a => a.Typ.Contains("nameidentifier")).FirstOrDefault()?.Val
                };
                var imageUrl = string.Format("http://graph.facebook.com/{0}/picture?type=large", loggedInAccount.SID);
                loggedInAccount.SocialToken = info.AccessToken;
                loggedInAccount.SocialProvider = info.ProviderName;
                loggedInAccount.Image = new Media
                {
                    Path = string.IsNullOrEmpty(account.Image.Path) ? imageUrl : account.Image.Path
                };

                _accountService.LoggedInAccount = loggedInAccount;
            }
            catch (Exception e)
            {
                throw;
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

                var authenticated = await App.Authenticator.Authenticate();
                if (authenticated == null)
                {
                    await DialogService.ShowMessageAsync("Cant login right now!", "Error");
                    return;
                }
                var client = _azureService.CreateOrGetAzureClient(authenticated.UserId,
                    authenticated.MobileServiceAuthenticationToken);

                FillUserData(client);
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