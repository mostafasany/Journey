using System.IO;
using Journey.ViewModels;
using Services.Core;
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
            //_viewModel?.OnNavigatedTo(null, false);
            base.OnAppearing();
        }

        private void LogPageView()
        {
            var pageName = Path.GetFileName(ToString());
            var loggerService = _viewModel?.Container.Resolve<ILoggerService>();
            loggerService?.LogPageView(pageName);
        }
        //    _viewModel?.OnNavigatedFrom();
        //{

        //protected override void OnDisappearing()
        //    base.OnDisappearing();
        //}
    }
}