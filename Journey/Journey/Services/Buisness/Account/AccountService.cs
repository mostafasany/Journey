using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Journey.Services.Buisness.Account.Data;
using Unity;

namespace Journey.Services.Buisness.Account
{
    public class AccountService : IAccountService
    {
        private const string AccountTokenKey = "AccountToken";
        private readonly IAccountDataService _accountDataService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;
        private readonly IExceptionService _exceptionService;

        public AccountService(IAccountDataService accountDataService,
            ISettingsService settingsService,
            INavigationService navigationService, IDialogService dialogService, IExceptionService exceptionService) 
        {
            _accountDataService = accountDataService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _settingsService = settingsService;
            _exceptionService = exceptionService;
        }

        public Tawasol.Models.Account LoggedInAccount { get; set; }

        public async Task<Tawasol.Models.Account> SaveAccountAsync(Tawasol.Models.Account account)
        {
            try
            {
                await _accountDataService.AddUpdateAccountAsync(account);
                return account;
            }
            catch (Exception ex)
            {
                _exceptionService.Handle(ex);
                return null;
            }
        }

        public async Task<Tawasol.Models.Account> GetAccountAsync(bool sync = false)
        {
            try
            {
                if (LoggedInAccount != null && !sync)
                    return LoggedInAccount;

                var account = await _settingsService.Get(AccountTokenKey);
                if (string.IsNullOrEmpty(account))
                    return null;

                LoggedInAccount = await _accountDataService.GetAccountAsync(sync);

                //if (LoggedInAccount != null)
                //    LoggedInAccount.AccountGoal = await accountGoalService.GetAccountGoalAsync(sync);

                return LoggedInAccount;
            }
            catch (Exception ex)
            {
                _exceptionService.Handle(ex);
                return null;
            }
        }

        public async Task<bool> LoginFirst()
        {
            try
            {
                var account = await _settingsService.Get(AccountTokenKey);
                if (string.IsNullOrEmpty(account))
                {
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
                }
                return true;
            }
            catch (Exception ex)
            {
                _exceptionService.Handle(ex);
                return false;
            }
        }

        public bool IsAccountAuthenticated()
        {
            return _accountDataService.IsAccountAuthenticated();
        }
    }
}