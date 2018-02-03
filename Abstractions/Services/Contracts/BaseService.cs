using Services.Core;
using Unity;

namespace Abstractions.Services.Contracts
{
    public class BaseService
    {
        protected readonly IUnityContainer Container;

        public BaseService(IUnityContainer container)
        {
            Container = container;
        }

        protected IExceptionService ExceptionService { get; set; }
        protected IHttpService HttpService { get; set; }
        protected ILoggerService LoggerService { get; set; }

        protected string Translate(string resource)
        {
            var resourceLoaderService = Container.Resolve<IResourceLoaderService>();
            var translatedResource = resourceLoaderService.GetString(resource);
            return translatedResource;
        }
    }
}