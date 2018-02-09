using Abstractions.Services;
using Abstractions.Services.Contracts;
using Journey.Constants;
using Journey.Models;
using Journey.Services.Azure;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Account.Data;
using Journey.Services.Buisness.Blob;
using Journey.Services.Buisness.Post;
using Journey.Services.Buisness.Post.Data;
using Journey.Services.Forms;
using Journey.Views;
using Microsoft.WindowsAzure.MobileServices;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using Unity;
using Unity.Lifetime;
using INavigationService = Abstractions.Services.Contracts.INavigationService;
using LoginPage = Journey.Views.LoginPage;
using UpdateProfilePage = Journey.Views.UpdateProfilePage;

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

        public static IAzureAuthenticateService Authenticator { get; private set; }
        public static MobileServiceClient Client { get; private set; }

        public static void Init(IAzureAuthenticateService authenticator)
        {
            Authenticator = authenticator;
            Client = new MobileServiceClient(Constant.ApplicationUrl);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var container = containerRegistry.GetContainer();
            containerRegistry.RegisterForNavigation<HomePage>();
            containerRegistry.RegisterForNavigation<LoginPage>();
            containerRegistry.RegisterForNavigation<UpdateProfilePage>();
            RegitserAppServices(container);

            RegitserBuisnessServices(container);
        }

        private void RegitserAppServices(IUnityContainer container)
        {
            container.RegisterInstance(typeof(IUnityContainer), container);
            container.RegisterType<IExceptionService, ExceptionService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IHttpService, HttpService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILoggerService, LoggerService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMediaService<Media>, MediaService>(new ContainerControlledLifetimeManager());
            container.RegisterType<INavigationService, NavigationService>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<Prism.Navigation.INavigationService, PageNavigationService>(
                new ContainerControlledLifetimeManager());

            container.RegisterType<ISerializerService, SerializerService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IShareService, ShareService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILocationService, LocationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISettingsService, SettingsService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IBlobService, BlobService>(new ContainerControlledLifetimeManager());

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
            container.RegisterType<IAzureService, AzureService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPostService, PostService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPostDataService, PostDataService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAccountDataService, AccountDataService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFacebookService, FacebookService>(new ContainerControlledLifetimeManager());
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            var settingsService = Container.Resolve<ISettingsService>();

            if (settingsService != null)
            {
                var azureService = Container.Resolve<IAzureService>();
                var accountService = Container.Resolve<IAccountService>();
                var userToken = await settingsService.Get(accountService.AccountTokenKey);
                var userId = await settingsService.Get(accountService.AccountIdKey);
                accountService.Token = userToken;

                azureService.CreateOrGetAzureClient(userId, userToken);

                await NavigationService.NavigateAsync(string.IsNullOrEmpty(userId) ? "LoginPage" : "HomePage");
            }
            else
            {
                await NavigationService.NavigateAsync("LoginPage");
            }
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