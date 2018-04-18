using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Contracts;
using Abstractions.Exceptions;
using Abstractions.Forms;
using Abstractions.Services.Contracts;
using Journey.Services.Buisness.Account.Data;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountDataService _accountDataService;

        private readonly IDialogService _dialogService;
        private readonly IFacebookService _facebookService;
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;

        public AccountService(IAccountDataService accountDataService, ISettingsService settingsService,
            INavigationService navService, IDialogService dialogService, IFacebookService facebookService)
        {
            _accountDataService = accountDataService;
            _navigationService = navService;
            _dialogService = dialogService;
            _settingsService = settingsService;
            _facebookService = facebookService;
        }

        public string AccountTokenKey { get; } = "AccountToken";
        public string AccountIdKey { get; } = "AccountId";

        public string Token { get; set; }

        public Models.Account.Account LoggedInAccount { get; set; }

        public async Task<Models.Account.Account> SaveAccountAsync(Models.Account.Account account, bool add)
        {
            try
            {
                //account.ChallengeId = "";
                Models.Account.Account savedAccount = await _accountDataService.AddUpdateAccountAsync(account, add);
                return savedAccount;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<Models.Account.Account> GetAccountAsync(bool sync = false)
        {
            try
            {
                if (LoggedInAccount != null && !sync)
                    return LoggedInAccount;

                if (string.IsNullOrEmpty(Token))
                    return null;


                LoggedInAccount = await _accountDataService.GetAccountAsync(sync);

                //if (LoggedInAccount != null)
                //    LoggedInAccount.AccountGoal = await accountGoalService.GetAccountGoalAsync(sync);

                return LoggedInAccount;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
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
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<MobileServiceUser> AutehticateAsync()
        {
            try
            {
                return await _accountDataService.AutehticateAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public async Task<bool> SoicalLoginAndSaveAsync(MobileServiceClient client)
        {
            try
            {
                List<Social> socialInfo = await _accountDataService.MeAsync();
                Social info = socialInfo.FirstOrDefault();
                Token = info.AccessToken;
                if (string.IsNullOrEmpty(Token))
                    return false;

                Models.Account.Account account = await GetAccountAsync(); //Save only if no new data , dont need to update everytime
                if (account == null)
                {
                    var loggedInAccount = new Models.Account.Account
                    {
                        FirstName = info.Claims?.Where(a => a.Typ.Contains("givenname")).FirstOrDefault()?.Val,
                        LastName = info.Claims?.Where(a => a.Typ.Contains("surname")).FirstOrDefault()?.Val,
                        Email = info.Claims?.Where(a => a.Typ.Contains("emailaddress")).FirstOrDefault()?.Val,
                        Gender = info.Claims?.Where(a => a.Typ.Contains("gender")).FirstOrDefault()?.Val,
                        SID = info.Claims?.Where(a => a.Typ.Contains("nameidentifier")).FirstOrDefault()?.Val
                    };
                    string imageUrl = string.Format("http://graph.facebook.com/{0}/picture?type=large",
                        loggedInAccount.SID);
                    loggedInAccount.SocialToken = info.AccessToken;
                    loggedInAccount.SocialProvider = info.ProviderName;
                    loggedInAccount.Image = new Media
                    {
                        Path = imageUrl
                    };
                    loggedInAccount.Id = client.CurrentUser.UserId;

                    LoggedInAccount = await SaveAccountAsync(loggedInAccount, true);
                }

                _facebookService.FacebookToken = info.AccessToken;
                await _settingsService.Set(_facebookService.FacebookTokenKey, info.AccessToken);
                await _settingsService.Set(AccountTokenKey, client.CurrentUser.MobileServiceAuthenticationToken);
                await _settingsService.Set(AccountIdKey, client.CurrentUser.UserId);
                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
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
                throw new BusinessException(ex.Message);
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                await App.Client.LogoutAsync();
                await _settingsService.Remove(_facebookService.FacebookTokenKey);
                await _settingsService.Remove(AccountTokenKey);
                await _settingsService.Remove(AccountIdKey);
                LoggedInAccount = null;
                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}