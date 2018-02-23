using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface INavigationService
    {
        string CurrentPage { get; set; }
        bool CanGoBack();
        bool CanGoForward();
        void ClearHistory();
        void GoBack(object parameter = null, string key = "");
        void GoForward();

        Task<bool> Navigate(string pageToken, object parameter = null, string key = "", bool? useModalNavigation = null,
            bool animated = false,bool removeLastPage=false);

        Task<bool> Navigate(string pageToken, Dictionary<string, object> parameters,
            bool? useModalNavigation = null, bool animated = false, bool removeLastPage = false);

        void RemoveAllPages(object parameter = null);
        void RemoveFirstPage();
        void RemoveLastPage();
        void Suspending();
    }
}