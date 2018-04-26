using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Abstractions.Contracts;
using Abstractions.Exceptions;
using Journey.Services.Azure;
using Journey.Services.Buisness.Account.Dto;
using Journey.Services.Buisness.Account.Translators;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Account.Data
{
    public class AccountDataService : IAccountDataService
    {
        private readonly IMobileServiceTable<AzureAccount> _accountTable;
        private readonly MobileServiceClient _client;

        public AccountDataService(IAzureService azureService)
        {
            _client = azureService.CreateOrGetAzureClient();
            _accountTable = _client.GetTable<AzureAccount>();
        }

        public async Task<Models.Account.Account> AddUpdateAccountAsync(Models.Account.Account account, bool add)
        {
            try
            {
                if (account == null)
                    return null;

                AzureAccount azureAccountDto = AccountDataTranslator.TranslateAccount(account);

                if (add)
                    await _accountTable.InsertAsync(azureAccountDto);

                else
                    await _accountTable.UpdateAsync(azureAccountDto);

                account = AccountDataTranslator.TranslateAccount(azureAccountDto);
                return account;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<MobileServiceUser> AutehticateAsync()
        {
            try
            {
                MobileServiceUser authenticated = await App.Authenticator.Authenticate();
                return authenticated;
            }
            catch (Exception)
            {
                throw new AppAuthenticationException();
            }
        }

        public async Task<Models.Account.Account> GetAccountAsync(bool sync = false)
        {
            Models.Account.Account accountDto = await GetAccontAsync(_client.CurrentUser.UserId);
            return accountDto;
        }

        public async Task<Models.Account.Account> GetAccontAsync(string id)
        {
            try
            {
                AzureAccount accountDto = await _accountTable.LookupAsync(id);
                Models.Account.Account account = AccountDataTranslator.TranslateAccount(accountDto);
                return account;
            }
            catch (HttpRequestException ex)
            {
                throw new NoInternetException(ex);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                    throw new DbItemNotFoundException(nameof(AzureAccount), ex);

                throw new DataServiceException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public bool IsAccountAuthenticated()
        {
            try
            {
                return _client?.CurrentUser != null &&
                       !string.IsNullOrEmpty(_client?.CurrentUser?.MobileServiceAuthenticationToken);
            }
            catch (Exception ex)
            {
                throw new DataServiceException(typeof(AppAuthenticationException), ex);
            }
        }

        public async Task<List<Social>> MeAsync()
        {
            try
            {
                List<Social> socialInfo = await _client.InvokeApiAsync<List<Social>>("/.auth/me");
                return socialInfo;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }
    }
}