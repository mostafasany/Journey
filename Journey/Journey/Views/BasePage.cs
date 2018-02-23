using System.IO;
using System.Linq;
using Abstractions.Services.Contracts;
using Journey.ViewModels;
using Unity;
using Xamarin.Forms;

namespace Journey.Views
{
    public class BasePage : ContentPage
    {
        private BaseViewModel _viewModel;

        protected override void OnAppearing()
        {
            _viewModel = BindingContext as BaseViewModel;
            //if(this.Navigation.ModalStack.Count()>0)
            //{
            //    var navigationService = _viewModel?.Container.Resolve<INavigationService>();
            //    navigationService.CurrentPage = this.Navigation.ModalStack?.LastOrDefault()?.ToString();
            //}

            LogPageView();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            if (Navigation.ModalStack.Count() > 0)
            {
                var navigationService = _viewModel?.Container.Resolve<INavigationService>();
                var page = Navigation.ModalStack?.LastOrDefault()?.ToString();
                navigationService.CurrentPage = page.Split(".".ToArray()).LastOrDefault();
            }
            base.OnDisappearing();
        }

        private void LogPageView()
        {
            var pageName = Path.GetFileName(ToString());
            var loggerService = _viewModel?.Container.Resolve<ILoggerService>();
            loggerService?.LogPageView(pageName);
        }
    }
}