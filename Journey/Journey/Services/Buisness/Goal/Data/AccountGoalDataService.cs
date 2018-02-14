using System;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Account;
using Journey.Services.Azure;
using Journey.Services.Buisness.Goal.Dto;
using Journey.Services.Buisness.Goal.Translators;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace Journey.Services.Buisness.Goal.Data
{
    public class AccountGoalDataService : IAccountGoalDataService
    {
        private readonly IMobileServiceSyncTable<AzureAccountGoal> _accountGoalTable;

        private readonly MobileServiceClient _client;

        public AccountGoalDataService(IAzureService azureService)
        {
            _client = azureService.CreateOrGetAzureClient();
            _accountGoalTable = _client.GetSyncTable<AzureAccountGoal>();
        }

        public async Task<AccountGoal> AddUpdateAccountGoalAsync(AccountGoal bodyWeight)
        {
            try
            {
                if (bodyWeight == null)
                    return null;

                var accountGoalDto =
                    AccountGoalDataTranslator.TranslateAccountGoal(bodyWeight, _client.CurrentUser.UserId);
                await _accountGoalTable.InsertAsync(accountGoalDto);

                //var syncedAccountGoalDto = await SyncGoalAsync();
                //if (syncedAccountGoalDto != null)
                //    accountGoalDto = syncedAccountGoalDto;

                bodyWeight = AccountGoalDataTranslator.TranslateAccountGoal(accountGoalDto);
                return bodyWeight;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<AccountGoal> GetAccountGoalAsync(bool sync = false)
        {
            try
            {
                //if (sync)
                //{
                //    returnedData = await SyncGoalAsync();
                //}

                //if (returnedData == null)
                //{
                var account = _client.CurrentUser.UserId;
                var returnedData = (await _accountGoalTable.CreateQuery().Where(acc => acc.Account == account)
                    .OrderByDescending(abc => abc.CreatedAt).ToListAsync()).FirstOrDefault();
                //}
                //if (returnedData == null)
                //{
                //    returnedData = await SyncGoalAsync();
                //}
                if (returnedData == null)
                    return null;

                var accountGoal = AccountGoalDataTranslator.TranslateAccountGoal(returnedData);
                return accountGoal;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }
        //    {

        //    try
        //    ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
        //{

        //public async Task<AzureAccountGoal> SyncGoalAsync()
        //        await this.Client.SyncContext.PushAsync();

        //        // The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
        //        // Use a different query name for each unique query in your program.
        //        string account = Client.CurrentUser.UserId;
        //        await this.accountGoalTable.PullAsync("allGoalItems", this.accountGoalTable.CreateQuery().Where(acc => acc.Account == account));
        //        //await this.accountGoalTable.PurgeAsync();

        //        var goal = (await accountGoalTable.CreateQuery().Where(acc => acc.Account == account).OrderByDescending(abc => abc.CreatedAt).ToListAsync());
        //        var lastGoal = goal.FirstOrDefault();
        //        return lastGoal;

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

        //            // Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
        //        }
        //    }
        //    return null;
        //}
    }
}