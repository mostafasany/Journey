using Journey.Views;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Services;
using Services.Core;
using Unity;
using Unity.Lifetime;

namespace Journey
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var container = containerRegistry.GetContainer();
            containerRegistry.RegisterForNavigation<HomePage>();

            RegitserAppServices(container);

            RegitserBuisnessServices(container);
        }

        private void RegitserAppServices(IUnityContainer container)
        {
            container.RegisterInstance(typeof(IUnityContainer), Container);
            container.RegisterType<IExceptionService, ExceptionService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IHttpService, HttpService>(new ContainerControlledLifetimeManager());

            //container.RegisterType<ILoggerService, LoggerService>(new ContainerControlledLifetimeManager());
            //container.RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager());
            //container.RegisterType<IResourceLoaderService, ResourceLoaderService>(
            //    new ContainerControlledLifetimeManager());
            //container.RegisterType<IPopupService, PopupService>(new ContainerControlledLifetimeManager());
            //container.RegisterType<IInternetService, InternetService>(new ContainerControlledLifetimeManager());
            //container.RegisterType<ILocalStorageService, LocalStorageService>(new ContainerControlledLifetimeManager());
            //container.RegisterType<ISerializerService, SerializerService>(new ContainerControlledLifetimeManager());


            //var popupService = Container.Resolve<IPopupService>() as PopupService;
            //popupService?.RegisterPopup("FilterPopup", typeof(FilterMovieUserControl));

            //ConfigureDialogService();

            //ConfigureRateReviewService();

            //ConfigureForceUpdateService();

            //ConfigurePlatformService();
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