﻿using System;
using System.Collections.Generic;
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

                //var existingaccount = await GetAccountAsync();
                //if (string.IsNullOrEmpty(account.FirstName)) //Means it came from Facebook Login "Not Data" so Migrate
                //    accountDto = AccountDataTranslator.TranslateAccount(existingaccount);
                if (add)
                    await _accountTable.InsertAsync(azureAccountDto);

                else
                    await _accountTable.UpdateAsync(azureAccountDto);


                //accountDto = await SyncAccountAsync();
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
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<Models.Account.Account> GetAccountAsync(bool sync = false)
        {
            //TODO: There shouldnt be two request to get user , it should call the account api and it returns challengeid if exist  
            try
            {
                if (string.IsNullOrEmpty(_client.CurrentUser.MobileServiceAuthenticationToken))
                    return null;

                AzureAccount azureAccountDto = await _accountTable.LookupAsync(_client.CurrentUser.UserId);
                //if (azureAccountDto != null && string.IsNullOrEmpty(azureAccountDto.Challenge))
                //    await _accountTable.UpdateAsync(azureAccountDto);

                //if (azureAccountDto == null)
                //    sync = true;

                //if (sync && azureAccountDTO == null)
                //{
                //    //Means i need it immediatley
                //    azureAccountDTO = await SyncAccountAsync();
                //}
                //else if (sync)
                //{
                //    SyncAccountAsync();
                //}

                Models.Account.Account accountDto = AccountDataTranslator.TranslateAccount(azureAccountDto);

                return accountDto;
            }
            catch (HttpRequestException ex)
            {
                throw new NoInternetException(ex);
            }
            catch (Exception)
            {
                //Means User not exists
                return null;
            }
        }

        public async Task<Models.Account.Account> GetAccontAsync(string id)
        {
            try
            {
                AzureAccount accountDto = await _accountTable.LookupAsync(id);
                Models.Account.Account account = AccountDataTranslator.TranslateAccount(accountDto);
                return account;
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
                return _client.CurrentUser != null &&
                       !string.IsNullOrEmpty(_client.CurrentUser.MobileServiceAuthenticationToken);
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
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

        public async Task<List<Models.Account.Account>> FindAccontAsync(string keyword)
        {
            try
            {
                string api = string.Format("Friends?action={0}", "notfriends");
                // var api = "friends";
                List<AzureAccount> accountTbl = await _client.InvokeApiAsync<List<AzureAccount>>(api, HttpMethod.Get, null);
                if (accountTbl != null)
                {
                    List<Models.Account.Account> accountDto = AccountDataTranslator.TranslateAccounts(accountTbl);
                    return accountDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }
    }
}