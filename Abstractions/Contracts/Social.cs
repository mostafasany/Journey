using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Abstractions.Contracts
{
    public class Social
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "provider_name")]
        public string ProviderName { get; set; }

        [JsonProperty(PropertyName = "expires_on")]
        public DateTime ExpireOn { get; set; }


        [JsonProperty(PropertyName = "user_claims")]
        public List<Claims> Claims { get; set; }
    }

    public class Claims
    {
        [JsonProperty(PropertyName = "typ")]
        public string Typ { get; set; }

        [JsonProperty(PropertyName = "val")]
        public string Val { get; set; }
    }
}