using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface INavigationService
    {
        bool CanGoBack();
        bool CanGoForward();
        void ClearHistory();
        void GoBack(object parameter = null, string key = "");
        void GoForward();

        Task<bool> Navigate(string pageToken, object parameter = null, string key = "", bool? useModalNavigation = null,
            bool animated = false);

        Task<bool> Navigate(string pageToken, Dictionary<string, object> parameters,
            bool? useModalNavigation = null, bool animated = false);

        void RemoveAllPages(string pageToken = null, object parameter = null);
        void RemoveFirstPage(string pageToken = null, object parameter = null);
        void RemoveLastPage(string pageToken = null, object parameter = null);
        void Suspending();
    }
}