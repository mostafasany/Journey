using Microsoft.Practices.Unity;
using Prism.Mvvm;
using Services.Core;

namespace ViewModels
{
    public class BaseViewModel : BindableBase
    {
        private bool _isLoading;

        protected BaseViewModel(IUnityContainer container)
        {
            Container = container;
            ExceptionService = Container.Resolve<IExceptionService>();
            PopupService = Container.Resolve<IPopupService>();
            NavigationService = Container.Resolve<INavigationService>();
            InternetService = Container.Resolve<IInternetService>();
            DialogService = Container.Resolve<IDialogService>();
            ResourceLoaderService = Container.Resolve<IResourceLoaderService>();
        }

        public IUnityContainer Container { get; set; }
        protected IExceptionService ExceptionService { get; set; }
        protected IPopupService PopupService { get; set; }
        protected INavigationService NavigationService { get; set; }
        protected IInternetService InternetService { get; set; }
        protected IDialogService DialogService { get; set; }
        protected IResourceLoaderService ResourceLoaderService { get; set; }
        protected ISerializerService SerializerService { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        protected void ShowProgress()
        {
            IsLoading = true;
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

        public virtual void OnNavigatedTo(object paramater, bool isBack)
        {
        }

        public virtual void OnNavigatedFrom()
        {
        }

        public virtual void OnNavigatingFrom()
        {
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