using System;
using System.Globalization;
using Journey.Views;
using Prism;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using Unity;

namespace Journey
{
    public partial class App : PrismApplication
    {
        private const string ViewModelNamespace = "Journey.ViewModels.{0}ViewModel,Journey";
        private const string UserControlViewModelNamespace = "ViewModels.UserControl.{0}ViewModel,ViewModels";
        private const string AppCenterSecret = "6db25340-d908-434c-a906-dcd8b914dbba";

        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var container = containerRegistry.GetContainer();
            containerRegistry.RegisterForNavigation<HomePage>();

            container.RegisterInstance(typeof(IUnityContainer), container);
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, ViewModelNamespace, viewType.Name);
                //if (viewType.Name.Contains("UserControl"))
                //    viewModelTypeName = string.Format(CultureInfo.InvariantCulture, UserControlViewModelNamespace,
                //        viewType.Name);
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });

            //Container.RegisterType<IPlacesService, PlacesService>(new ContainerControlledLifetimeManager());
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