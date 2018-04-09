using Journey.Constants;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Azure
{
    public class AzureService : IAzureService
    {
        private MobileServiceClient _client;
        public const string OfflineDbPath = @"localstore.db";

        public MobileServiceClient CreateOrGetAzureClient(string id, string token)
        {
            if (_client == null)
                _client = new MobileServiceClient(Constant.ApplicationUrl);

            if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(token))
                _client.CurrentUser = new MobileServiceUser(id)
                {
                    MobileServiceAuthenticationToken = token
                };
            return _client;
        }

        private static void DefineStore(MobileServiceClient client)
        {
            //var store = new MobileServiceSQLiteStore(OfflineDbPath);
            //store.DefineTable<AzureAccount>();
            //store.DefineTable<AzureAccountGoal>();
            //store.DefineTable<AzureAccountMeasurements>();
            //store.DefineTable<AzurePostComments>();
            //store.DefineTable<AzurePost>();
            //  client.SyncContext.InitializeAsync(store);
        }
    }
}