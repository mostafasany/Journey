using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Tawasol.Azure.Models
{
    public class AzureFriends
    {
        string id;
        string account1;
        string account2;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "account1")]
        public string Accoun1
        {
            get { return account1; }
            set { account1 = value; }
        }

        [JsonProperty(PropertyName = "account2")]
        public string Account2
        {
            get { return account2; }
            set { account2 = value; }
        }

        [Deleted]
        [JsonIgnore]
        public bool deleted { get; set; }
    }
}
