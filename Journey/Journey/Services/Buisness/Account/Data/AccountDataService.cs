using System;
using System.Threading.Tasks;
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

                var accountDto = AccountDataTranslator.TranslateAccount(account);

                //var existingaccount = await GetAccountAsync();
                //if (string.IsNullOrEmpty(account.FirstName)) //Means it came from Facebook Login "Not Data" so Migrate
                //    accountDto = AccountDataTranslator.TranslateAccount(existingaccount);
                if (add)
                    await _accountTable.InsertAsync(accountDto);

                else
                    await _accountTable.UpdateAsync(accountDto);

                //accountDto = await SyncAccountAsync();
                account = AccountDataTranslator.TranslateAccount(accountDto);
                return account;
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

                var azureAccountDto = await _accountTable.LookupAsync(_client.CurrentUser.UserId);

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

                var accountDto = AccountDataTranslator.TranslateAccount(azureAccountDto);

                return accountDto;
            }
            catch (Exception)
            {
                return null;
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
        //    {

        //    try
        //    ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
        //{

        //private async Task<AzureAccount> SyncAccountAsync()
        //        await this.Client.SyncContext.PushAsync();


        //        // The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
        //        // Use a different query name for each unique query in your program.
        //        await this.accountTable.PullAsync("account", this.accountTable.CreateQuery());

        //        string account = Client.CurrentUser.UserId;
        //        AzureAccount azureAccountDTO = await accountTable.LookupAsync(account);
        //        if (azureAccountDTO != null)
        //        {
        //            var challenge = await challengeDataService.GetAccountChallengeAsync();
        //            azureAccountDTO.Challenge = challenge?.Id;
        //            await this.accountTable.UpdateAsync(azureAccountDTO);

        //            var accountDTO = AccountDataTranslator.TranslateAccount(azureAccountDTO);
        //            AccountSynced?.Invoke(this, new AccountSyncedEventArgs
        //            {
        //                Account = accountDTO,
        //                Updated = DateTime.Now,
        //                Error = null,
        //            });
        //            return azureAccountDTO;
        //        }
        //    }
        //    catch (MobileServicePushFailedException exc)
        //    {
        //        ExceptionService.Handle(exc);
        //        if (exc.PushResult != null)
        //        {
        //            syncErrors = exc.PushResult.Errors;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionService.Handle(ex);
        //        return null;
        //    }

        //    // Simple error/conflict handling.
        //    if (syncErrors != null)
        //    {
        //        foreach (var error in syncErrors)
        //        {
        //            if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
        //            {
        //                // Update failed, revert to server's copy
        //                await error.CancelAndUpdateItemAsync(error.Result);
        //            }
        //            else
        //            {
        //                // Discard local change
        //                await error.CancelAndDiscardItemAsync();
        //            }
        //        }
        //        //If Conflicets Happens , Resolve it then query again
        //        string account = Client.CurrentUser.UserId;
        //        AzureAccount azureAccountDTO = await accountTable.LookupAsync(account);
        //        if (azureAccountDTO != null)
        //        {
        //            var accountDTO = AccountDataTranslator.TranslateAccount(azureAccountDTO);
        //            //AccountSynced?.Invoke(this, new AccountSyncedEventArgs
        //            //{
        //            //    Account = accountDTO,
        //            //    Updated = DateTime.Now,
        //            //    Error = null,
        //            //});
        //            return azureAccountDTO;
        //        }
        //    }
        //    return null;
        //}
    }
}