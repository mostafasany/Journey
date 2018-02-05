using System.IO;
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
            LogPageView();
            base.OnAppearing();
        }

        private void LogPageView()
        {
            var pageName = Path.GetFileName(ToString());
            var loggerService = _viewModel?.Container.Resolve<ILoggerService>();
            loggerService?.LogPageView(pageName);
        }
    }
}