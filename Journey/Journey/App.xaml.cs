using Prism;
using Prism.Ioc;
using Prism.Unity;

namespace Journey
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainPage>();
            //Container.RegisterTypeForNavigation<HomePage>();
            //Container.RegisterTypeForNavigation<FloorPage>();
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
            //Container.RegisterType<IPlacesService, PlacesService>(new ContainerControlledLifetimeManager());
            NavigationService.NavigateAsync("MainPage");
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