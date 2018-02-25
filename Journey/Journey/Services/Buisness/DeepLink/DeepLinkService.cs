using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abstractions.Exceptions;
using Abstractions.Services;
using Abstractions.Services.Contracts;

namespace Journey.Services.Buisness.DeepLink
{
    public class DeepLinkService : IDeepLinkService
    {
        INavigationService navigationService;
        public DeepLinkService(INavigationService _navigationService)
        {
            navigationService = _navigationService;
        }
        private const string ProfilePageName = "profile";
        private const string ChallengeRequestName = "challengerequest";
        private const string OurDomainName = "journey";

        public void ParseDeepLinkingAndExecute(string deepLink)
        {
            try
            {
                if (string.IsNullOrEmpty(deepLink))
                    return;
                Uri url = new Uri(deepLink);
                string domain = url.Host;
                if (domain.Contains(OurDomainName))
                {
                    string pathAndQuery = url.PathAndQuery.Replace("/?", "");
                    var paramaters = HttpUtility.ParseQueryString(pathAndQuery);
                    var hostParts = url.Host.Split('.').ToList();
                    string pageName = hostParts[2];
                    if (pageName == ProfilePageName)
                    {
                        navigationService.Navigate("ProfileChallengePage");
                    }
                    else if (pageName == ChallengeRequestName)
                    {
                        string id = paramaters["id"];
                        var parameters = new Dictionary<string, object>
                         {
                            {"Challenge", id},
                            {"Mode", 2}
                         };
                        navigationService.Navigate("NewChallengePage", parameters);
                    }
                }
                else
                {
                    //navigationService.Navigate(typeof(WebViewPage), url);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);

            }
        }
    }
}
