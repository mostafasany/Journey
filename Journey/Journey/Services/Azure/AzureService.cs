using Journey.Constants;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Azure
{
    public class AzureService : IAzureService
    {
        private MobileServiceClient _client;

        public MobileServiceClient CreateOrGetAzureClient(string id, string token)
        {
            return _client ?? (_client = new MobileServiceClient(Constant.ApplicationUrl)
            {
                CurrentUser = new MobileServiceUser(id)
                {
                    MobileServiceAuthenticationToken = token
                }
            });
        }
    }
}