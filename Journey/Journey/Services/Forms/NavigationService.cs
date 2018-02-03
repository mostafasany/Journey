﻿//using System;
//using System.Threading.Tasks;
//using Exceptions;
//using Prism.Navigation;
//using Unity;
//using INavigationService = Abstractions.Services.Contracts.INavigationService;

//namespace Journey.Services.Forms
//{
//    public class NavigationService : INavigationService
//    {
//        private readonly Prism.Navigation.INavigationService _navigationService;

//        public NavigationService(IUnityContainer container) //:base(container)
//        {
//            var c = container.Resolve<Prism.Navigation.INavigationService>();
//        }

//        public bool CanGoBack()
//        {
//            throw new NotImplementedException();
//        }

//        public bool CanGoForward()
//        {
//            throw new NotImplementedException();
//        }

//        public void ClearHistory()
//        {
//            throw new NotImplementedException();
//        }

//        public void GoBack()
//        {
//            try
//            {
//                _navigationService.GoBackAsync();
//            }
//            catch (Exception ex)
//            {
//                throw new CoreServiceException(ex);
//            }
//        }

//        public void GoForward()
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<bool> Navigate(string pageToken, object parameter)
//        {
//            try
//            {
//                await _navigationService.NavigateAsync(pageToken, parameter as NavigationParameters);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                throw new CoreServiceException(ex);
//            }
//        }

//        public void RemoveAllPages(string pageToken = null, object parameter = null)
//        {
//            try
//            {
//                _navigationService.GoBackToRootAsync(parameter as NavigationParameters);
//            }
//            catch (Exception ex)
//            {
//                throw new CoreServiceException(ex);
//            }
//        }

//        public void RemoveFirstPage(string pageToken = null, object parameter = null)
//        {
//            throw new NotImplementedException();
//        }

//        public void RemoveLastPage(string pageToken = null, object parameter = null)
//        {
//            throw new NotImplementedException();
//        }

//        public void Suspending()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

