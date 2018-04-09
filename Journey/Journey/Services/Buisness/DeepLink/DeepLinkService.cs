using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Abstractions.Exceptions;
using Abstractions.Services;
using Abstractions.Services.Contracts;

namespace Journey.Services.Buisness.DeepLink
{
    public class DeepLinkService : IDeepLinkService
    {
        private readonly INavigationService navigationService;
        private const string ProfilePageName = "profile";
        private const string ChallengeRequestName = "challengerequest";
        private const string OurDomainName = "journey";

        public DeepLinkService(INavigationService _navigationService) => navigationService = _navigationService;

        public void ParseDeepLinkingAndExecute(string deepLink)
        {
            try
            {
                if (string.IsNullOrEmpty(deepLink))
                    return;
                var url = new Uri(deepLink);
                string domain = url.Host;
                if (domain.Contains(OurDomainName))
                {
                    string pathAndQuery = url.PathAndQuery.Replace("/?", "");
                    NameValueCollection paramaters = HttpUtility.ParseQueryString(pathAndQuery);
                    List<string> hostParts = url.Host.Split('.').ToList();
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
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}