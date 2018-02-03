using System;
using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Exceptions;
using Models;
using Prism.Navigation;
using Services.Core;
using Unity;

namespace Abstractions.Services
{
    public class ForceUpdateService : BaseService, IForceUpdateService
    {
        private readonly INavigationService _navigationService;
        private readonly ISerializerService _serializerService;

        public ForceUpdateService(IUnityContainer container, IHttpService httpService,
            ISerializerService serializerService,
            INavigationService navigationService, ISerializerService serializerService1) : base(container)
        {
            HttpService = httpService;
            _navigationService = navigationService;
            _serializerService = serializerService1;
        }

        public string ForceUpdatePageKey { get; set; }
        public string API { get; set; }

        public async Task<bool> CheckForceUpdateAsync()
        {
            try
            {
                //var url = string.Format(API);
                //var forceHttpResult = await HttpService.HttpPostAsync<Contracts.ForceUpdate>(url,null);
                //var result = forceHttpResult.Result;
                //if (result == null)
                //    return;

                var result = new ForceUpdate
                {
                    IsForceUpdate = false,
                    IsShutdown = false,
                    Message = "Update to our latest features"
                };
                if (result.IsForceUpdate || result.IsShutdown)
                {
                    var parameters =
                        new NavigationParameters {{"ForceUpdateResults", _serializerService.SerializeToString(result)}};
                    await _navigationService.GoBackToRootAsync();
                    await _navigationService.NavigateAsync(ForceUpdatePageKey, parameters);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }
    }
}