using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models;
using Journey.Models.Post;
using Journey.Services.Azure;
using Journey.Services.Buisness.Measurment.Dto;
using Journey.Services.Buisness.Measurment.Translators;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Measurment.Data
{
    public class AccountMeasurmentDataService : IAccountMeasurmentDataService
    {
        readonly IMobileServiceTable<AzureAccountMeasurements> _accountMeasurementsTable;
        private readonly MobileServiceClient _client;

        public AccountMeasurmentDataService(IAzureService azureService)
        {
            _client = azureService.CreateOrGetAzureClient();
            _accountMeasurementsTable = _client.GetTable<AzureAccountMeasurements>();
        }

        public async Task<List<ScaleMeasurment>> AddUpdateAccountMeasurmentAsync(List<ScaleMeasurment> accountMeasurments)
        {
            try
            {
                if (accountMeasurments == null)
                    return null;

                string account = _client.CurrentUser.UserId;
                AzureAccountMeasurements accountMeasureDto = AccountMeasurmentDataTranslator.TranslateAccountMeasurments(account, accountMeasurments);

                await _accountMeasurementsTable.InsertAsync(accountMeasureDto);

                //var syncedData = await SyncMeasurmentsAsync();

                //if (syncedData != null)
                //    syncedData = accountMeasureDto;

                accountMeasurments = AccountMeasurmentDataTranslator.TranslateAccountMeasurments(account, accountMeasureDto);

                return accountMeasurments;
            }
            catch (System.Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<List<ScaleMeasurment>> GetAccountMeasurmentAsync(bool sync = false)
        {
            try
            {
                //if (sync)
                //{
                //    returnedData = await SyncMeasurmentsAsync();
                //}
                //if (returnedData == null)
                //{
                    string account = _client.CurrentUser.UserId;
                    var returnedData = (await _accountMeasurementsTable.CreateQuery().Where(acc => acc.Account == account).OrderByDescending(acc => acc.CreatedAt).ToListAsync())?.FirstOrDefault();
                //}
                //if (returnedData == null)
                //{
                //    returnedData = await SyncMeasurmentsAsync();
                //}
                if (returnedData == null)
                {
                    return await GetEmptyMeasurmentsAsync();
                }

                var measuremnts = AccountMeasurmentDataTranslator.TranslateAccountMeasurments(_client.CurrentUser.UserId, returnedData);
                return measuremnts;

            }
            catch (System.Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        private async Task<List<ScaleMeasurment>> GetEmptyMeasurmentsAsync()
        {
            List<ScaleMeasurment> measuremnts = new List<ScaleMeasurment>
            {
                new ScaleMeasurment
                {
                    UpIndictor = true,
                    Title = "Weight",
                    Unit = "KG",
                    Measure = 0,
                    Image = new Media {Path = "http://bit.ly/2z9lPaA"}
                },
                new ScaleMeasurment
                {
                    UpIndictor = true,
                    Title = "Muscle",
                    Unit = "%",
                    Measure = 0,
                    Image = new Media {Path = "http://bit.ly/2gEeJ2t"}
                },
                new ScaleMeasurment
                {
                    UpIndictor = false,
                    Title = "Fat",
                    Unit = "%",
                    Measure = 0,
                    Image = new Media {Path = "http://bit.ly/2xnWzbZ"}
                },
                new ScaleMeasurment
                {
                    UpIndictor = false,
                    Title = "BMI",
                    Unit = "",
                    Measure = 0,
                    Image = new Media {Path = "http://bit.ly/2yRyMER"}
                },
                new ScaleMeasurment
                {
                    UpIndictor = true,
                    Title = "BMR",
                    Unit = "Kcal",
                    Measure = 0,
                    Image = new Media {Path = "http://bit.ly/2yRzwd7"}
                },
                new ScaleMeasurment
                {
                    UpIndictor = true,
                    Title = "Water",
                    Unit = "%",
                    Measure = 0,
                    Image = new Media {Path = "http://bit.ly/2yR2J9j"}
                }
            };

            return measuremnts;
        }



        //public async Task<AzureAccountMeasurements> SyncMeasurmentsAsync()
        //{
        //    ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

        //    try
        //    {
        //        await this.Client.SyncContext.PushAsync();

        //        // The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
        //        // Use a different query name for each unique query in your program.
        //        await this.accountMeasurementsTable.PullAsync("allMeasurmentsItems", this.accountMeasurementsTable.CreateQuery());

        //        string account = Client.CurrentUser.UserId;
        //        var measu = (await accountMeasurementsTable.CreateQuery().Where(acc => acc.Account == account).OrderByDescending(acc => acc.CreatedAt).ToListAsync())?.FirstOrDefault();
        //        return measu;
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
