using System;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.Challenge.Dto
{
    public class AzureChallenge
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "account1")]
        public string Account1 { get; set; }

        [JsonProperty(PropertyName = "account2")]
        public string Account2 { get; set; }

        [JsonProperty(PropertyName = "start")]
        public DateTime Start { get; set; }

        [JsonProperty(PropertyName = "end")]
        public DateTime End { get; set; }

        [JsonProperty(PropertyName = "status")]
        public bool Status { get; set; }

        [JsonProperty(PropertyName = "terms")]
        public string Terms { get; set; }

    }
}