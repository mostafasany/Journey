using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Abstractions.Exceptions;
using Abstractions.Services;

namespace Journey.Services.Buisness.DeepLink
{
    public class DeepLinkService : IDeepLinkService
    {
        private const string ProfilePageName = "profile";
        private const string ChallengeRequestName = "challengerequest";
        private const string OurDomainName = "journey";


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
                        // _navigationService.Navigate("ChallengeProgressPage");
                    }
                    else if (pageName == ChallengeRequestName)
                    {
                        string id = paramaters["id"];
                        var parameters = new Dictionary<string, object>
                        {
                            {"Challenge", id},
                            {"Mode", 2}
                        };
                        //  _navigationService.Navigate("NewChallengePage", parameters);
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