using Abstractions.Services.Contracts;
using Prism.Mvvm;
using Unity;

namespace Journey.ViewModels
{
    public class BaseViewModel : BindableBase
    {
        private bool _isLoading;

        protected BaseViewModel(IUnityContainer container)
        {
            Container = container;

            if (Container.IsRegistered<IExceptionService>())
                ExceptionService = Container.Resolve<IExceptionService>();
            if (Container.IsRegistered<IPopupService>())
                PopupService = Container.Resolve<IPopupService>();
            if (Container.IsRegistered<INavigationService>())
                NavigationService = Container.Resolve<INavigationService>();
            if (Container.IsRegistered<IInternetService>())
                InternetService = Container.Resolve<IInternetService>();
            if (Container.IsRegistered<IDialogService>())
                DialogService = Container.Resolve<IDialogService>();
            if (Container.IsRegistered<IResourceLoaderService>())
                ResourceLoaderService = Container.Resolve<IResourceLoaderService>();
        }

        public IUnityContainer Container { get; set; }

        protected IExceptionService ExceptionService { get; set; }
        protected IPopupService PopupService { get; set; }
        protected INavigationService NavigationService { get; set; }
        protected IInternetService InternetService { get; set; }
        protected IDialogService DialogService { get; set; }
        protected IResourceLoaderService ResourceLoaderService { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            private set => SetProperty(ref _isLoading, value);
        }

        protected void ShowProgress()
        {
            IsLoading = true;
        }

        protected bool IsProgress()
        {
            return IsLoading;
        }

        protected void HideProgress()
        {
            IsLoading = false;
        }

        protected string Translate(string resource)
        {
            var translatedResource = ResourceLoaderService.GetString(resource);
            return translatedResource;
        }


        public virtual void OnBackPressed()
        {
        }

        protected virtual void Cleanup()
        {
        }

        public virtual void Intialize()
        {
        }
    }
}