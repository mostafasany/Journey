using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Abstractions.Contracts
{
    public class Social
    {
        string access_token;
        string provider_name;
        DateTime expires_on;
        List<Claims> user_claims;

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken
        {
            get { return access_token; }
            set { access_token = value; }
        }

        [JsonProperty(PropertyName = "provider_name")]
        public string ProviderName
        {
            get { return provider_name; }
            set { provider_name = value; }
        }

        [JsonProperty(PropertyName = "expires_on")]
        public DateTime ExpireOn
        {
            get { return expires_on; }
            set { expires_on = value; }
        }


        [JsonProperty(PropertyName = "user_claims")]
        public List<Claims> Claims
        {
            get { return user_claims; }
            set { user_claims = value; }
        }

    }

    public class Claims
    {
        string typ;
        string val;



        [JsonProperty(PropertyName = "typ")]
        public string Typ
        {
            get { return typ; }
            set { typ = value; }
        }

        [JsonProperty(PropertyName = "val")]
        public string Val
        {
            get { return val; }
            set { val = value; }
        }



    }
}
