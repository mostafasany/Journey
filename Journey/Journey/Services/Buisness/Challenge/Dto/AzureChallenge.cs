using System;
using Newtonsoft.Json;

namespace Tawasol.Azure.Models
{
    public class AzureChallenge
    {
        string id;
        string account1;
        string account2;
        DateTime start;
        DateTime end;
        bool status;
        string terms;


        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "account1")]
        public string Account1
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

        [JsonProperty(PropertyName = "start")]
        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }

        [JsonProperty(PropertyName = "end")]
        public DateTime End
        {
            get { return end; }
            set { end = value; }
        }

        [JsonProperty(PropertyName = "status")]
        public bool Status
        {
            get { return status; }
            set { status = value; }
        }

        [JsonProperty(PropertyName = "terms")]
        public string Terms
        {
            get { return terms; }
            set { terms = value; }
        }
    }


}
