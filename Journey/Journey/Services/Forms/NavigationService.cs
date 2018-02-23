using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Prism.Navigation;
using Unity;
using INavigationService = Abstractions.Services.Contracts.INavigationService;

namespace Journey.Services.Forms
{
    public class NavigationService : INavigationService
    {
        public string CurrentPage { get; set; }

        private readonly Prism.Navigation.INavigationService _navigationService;

        public NavigationService(IUnityContainer container) //:base(container)
        {
            _navigationService = container.Resolve<Prism.Navigation.INavigationService>();
        }

        public bool CanGoBack()
        {
            throw new NotImplementedException();
        }

        public bool CanGoForward()
        {
            throw new NotImplementedException();
        }

        public void ClearHistory()
        {
            try
            {
                _navigationService.GoBackToRootAsync();
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex);
            }
        }

        public void GoBack(object parameter = null, string key = "")
        {
            try
            {
                NavigationParameters navigationParameters = null;
                if (parameter != null)
                    navigationParameters = new NavigationParameters
                    {
                        {key, parameter}
                    };
                _navigationService.GoBackAsync(navigationParameters);
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex);
            }
        }

        public void GoForward()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Navigate(string pageToken, object parameter = null, string key = "",
            bool? useModalNavigation = null, bool animated = false, bool removeLastPage = false)
        {
            try
            {
                NavigationParameters navigationParameters = null;
                if (parameter != null)
                    navigationParameters = new NavigationParameters
                    {
                        {key, parameter}
                    };

                await Navigate(pageToken, useModalNavigation, animated, removeLastPage, navigationParameters);

                return true;
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex);
            }
        }

        public async Task<bool> Navigate(string pageToken, Dictionary<string, object> parameters,
            bool? useModalNavigation = null, bool animated = false, bool removeLastPage = false)
        {
            try
            {
                NavigationParameters navigationParameters = null;
                if (parameters != null)
                {
                    navigationParameters = new NavigationParameters();
                    foreach (var parameter in parameters)
                        navigationParameters.Add(parameter.Key, parameter.Value);
                }

                await Navigate(pageToken, useModalNavigation, animated, removeLastPage, navigationParameters);

                return true;
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex);
            }
        }

        private async Task Navigate(string pageToken, bool? useModalNavigation, bool animated, bool removeLastPage,
            NavigationParameters navigationParameters)
        {
            var lastPage = App.Current.MainPage.Navigation.ModalStack.LastOrDefault();
            await _navigationService.NavigateAsync(pageToken, navigationParameters, useModalNavigation, animated);
            if (removeLastPage)
                App.Current.MainPage.Navigation.RemovePage(lastPage);
        }

        public void RemoveAllPages(object parameter = null)
        {
            try
            {
                _navigationService.GoBackToRootAsync(parameter as NavigationParameters);
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex);
            }
        }

        public void RemoveFirstPage()
        {
            throw new NotImplementedException();
        }

        public void RemoveLastPage()
        {
            throw new NotImplementedException();
        }

        public void Suspending()
        {
            throw new NotImplementedException();
        }
    }
}