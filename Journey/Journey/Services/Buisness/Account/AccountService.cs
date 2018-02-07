using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Contracts;
using Abstractions.Exceptions;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Journey.Services.Buisness.Account.Data;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountDataService _accountDataService;

        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;

        public AccountService(IAccountDataService accountDataService, ISettingsService settingsService,
            INavigationService navService, IDialogService dialogService)
        {
            _accountDataService = accountDataService;
            _navigationService = navService;
            _dialogService = dialogService;
            _settingsService = settingsService;
        }

        public string AccountTokenKey { get; } = "AccountToken";
        public string Token { get; set; }
        public Models.Account.Account LoggedInAccount { get; set; }

        public async Task<Models.Account.Account> SaveAccountAsync(Models.Account.Account account)
        {
            try
            {
               var savedAccount= await _accountDataService.AddUpdateAccountAsync(account);
                return savedAccount;
            }
            catch (Exception ex)
            {
                throw new BuisnessException(ex.Message);
            }
        }

        public async Task<Models.Account.Account> GetAccountAsync(bool sync = false)
        {
            try
            {
                //if (LoggedInAccount != null && !sync)
                //    return LoggedInAccount;

                if (string.IsNullOrEmpty(Token))
                    return null;

                LoggedInAccount = await _accountDataService.GetAccountAsync(sync);

                //if (LoggedInAccount != null)
                //    LoggedInAccount.AccountGoal = await accountGoalService.GetAccountGoalAsync(sync);

                return LoggedInAccount;
            }
            catch (Exception ex)
            {
                throw new BuisnessException(ex.Message);
            }
        }

        public async Task<bool> LoginFirstAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(Token)) return true;

                var commands =
                    new List<DialogCommand>
                    {
                        new DialogCommand
                        {
                            Label = "Ok",
                            Invoked = () => { _navigationService.Navigate("LoginPage"); }
                        },
                        new DialogCommand
                        {
                            Label = "Cancel"
                        }
                    };
                await _dialogService.ShowMessageAsync("Login!", "Do you want to login", commands);
                return true;
            }
            catch (Exception ex)
            {
                throw new BuisnessException(ex.Message);
            }
        }

        public async Task<bool> SoicalLoginAndSaveAsync(MobileServiceClient client)
        {
            try
            {
                var socialInfo = await client.InvokeApiAsync<List<Social>>("/.auth/me");
                var info = socialInfo.FirstOrDefault();
                Token = info.AccessToken;
                if (string.IsNullOrEmpty(Token))
                    return false;
               
                var account = await GetAccountAsync();
               
                var loggedInAccount = new Models.Account.Account
                {
                    FirstName = string.IsNullOrEmpty(account?.FirstName)
                        ? info.Claims?.Where(a => a.Typ.Contains("givenname")).FirstOrDefault()?.Val
                        : account?.FirstName,
                    LastName = string.IsNullOrEmpty(account?.LastName)
                        ? info.Claims?.Where(a => a.Typ.Contains("surname")).FirstOrDefault()?.Val
                        : account?.LastName,
                    Email = info.Claims?.Where(a => a.Typ.Contains("emailaddress")).FirstOrDefault()?.Val,
                    Gender = info.Claims?.Where(a => a.Typ.Contains("gender")).FirstOrDefault()?.Val,
                    SID = info.Claims?.Where(a => a.Typ.Contains("nameidentifier")).FirstOrDefault()?.Val
                };
                var imageUrl = string.Format("http://graph.facebook.com/{0}/picture?type=large", loggedInAccount.SID);
                loggedInAccount.SocialToken = info.AccessToken;
                loggedInAccount.SocialProvider = info.ProviderName;
                loggedInAccount.Image = new Media
                {
                    Path = string.IsNullOrEmpty(account?.Image?.Path) ? imageUrl : account?.Image?.Path
                };

                LoggedInAccount = await SaveAccountAsync(loggedInAccount);
                await _settingsService.Set(AccountTokenKey, Token);
                return true;
            }
            catch (Exception ex)
            {
                throw new BuisnessException(ex.Message);
            }
        }

        public bool IsAccountAuthenticated()
        {
            try
            {
                return _accountDataService.IsAccountAuthenticated();
            }
            catch (Exception ex)
            {
                throw new BuisnessException(ex.Message);
            }
        }
    }
}