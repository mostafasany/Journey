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
        private const string ProfilePageName = "profile";
        private const string ChallengeRequestName = "challengerequest";
        private const string OurDomainName = "journey";
        private readonly INavigationService navigationService;

        public DeepLinkService(INavigationService _navigationService)
        {
            navigationService = _navigationService;
        }

        public void ParseDeepLinkingAndExecute(string deepLink)
        {
            try
            {
                if (string.IsNullOrEmpty(deepLink))
                    return;
                var url = new Uri(deepLink);
                var domain = url.Host;
                if (domain.Contains(OurDomainName))
                {
                    var pathAndQuery = url.PathAndQuery.Replace("/?", "");
                    var paramaters = HttpUtility.ParseQueryString(pathAndQuery);
                    var hostParts = url.Host.Split('.').ToList();
                    var pageName = hostParts[2];
                    if (pageName == ProfilePageName)
                    {
                        navigationService.Navigate("ProfileChallengePage");
                    }
                    else if (pageName == ChallengeRequestName)
                    {
                        var id = paramaters["id"];
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