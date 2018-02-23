using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.Friend.Dto
{
    public class AzureFriends
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "account1")]
        public string Accoun1 { get; set; }

        [JsonProperty(PropertyName = "account2")]
        public string Account2 { get; set; }

        [Deleted]
        [JsonIgnore]
        public bool deleted { get; set; }
    }
}