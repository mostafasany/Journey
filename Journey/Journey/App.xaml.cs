using Abstractions.Services;
using Journey.Services.Forms;
using Journey.Views;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using Services.Core;
using Unity;
using Unity.Lifetime;

namespace Journey
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
            //Prism.Common.ApplicationProvider
            // Prism.AppModel.ApplicationStore
            //DeviceService
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var container = containerRegistry.GetContainer();
            containerRegistry.RegisterForNavigation<HomePage>();
            containerRegistry.RegisterForNavigation<LoginPage>();

            RegitserAppServices(container);

            RegitserBuisnessServices(container);
        }

        private void RegitserAppServices(IUnityContainer container)
        {
            container.RegisterInstance(typeof(IUnityContainer), container);
            container.RegisterType<IExceptionService, ExceptionService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IHttpService, HttpService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILoggerService, LoggerService>(new ContainerControlledLifetimeManager());
            container.RegisterType<INavigationService, PageNavigationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<Abstractions.Services.Contracts.INavigationService, NavigationService>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<ISerializerService, SerializerService>(new ContainerControlledLifetimeManager());

            //container.RegisterType<IResourceLoaderService, ResourceLoaderService>(
            //    new ContainerControlledLifetimeManager());
            //container.RegisterType<IPopupService, PopupService>(new ContainerControlledLifetimeManager());
            //container.RegisterType<IInternetService, InternetService>(new ContainerControlledLifetimeManager());
            //container.RegisterType<ILocalStorageService, LocalStorageService>(new ContainerControlledLifetimeManager());

            //var popupService = Container.Resolve<IPopupService>() as PopupService;
            //popupService?.RegisterPopup("FilterPopup", typeof(FilterMovieUserControl));

            ConfigureDialogService(container);

            //ConfigureRateReviewService();

            //ConfigureForceUpdateService();

            //ConfigurePlatformService();
        }

        private void ConfigureDialogService(IUnityContainer container)
        {
            container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            var dialogService = Container.Resolve<IDialogService>() as DialogService;
            if (dialogService != null)
            {
                dialogService.ErrorMessageTitle = "Error Occured";
                dialogService.ErrorMessageBody = "Please try again later";
                dialogService.NoInternetMessageBody = "No internet";
                dialogService.NoInternetMessageTitle =
                    "No internet connection available,Please reconnect and try again later";
            }
        }


        private void RegitserBuisnessServices(IUnityContainer container)
        {
            // container.RegisterType<IPlacesService, PlacesService>(new ContainerControlledLifetimeManager());
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("HomePage");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}