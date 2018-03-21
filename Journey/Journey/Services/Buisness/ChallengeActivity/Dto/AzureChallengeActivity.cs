using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.ChallengeActivity.Dto
{
    public class AzureChallengeActivity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; }

        [JsonProperty(PropertyName = "challenge")]
        public string Challenge { get; set; }

        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }

        [JsonProperty(PropertyName = "activity")]
        public string Activity { get; set; }

        [JsonProperty(PropertyName = "fname")]
        public string Fname { get; set; }

        [JsonProperty(PropertyName = "lname")]
        public string Lname { get; set; }

        [JsonProperty(PropertyName = "profile")]
        public string Profile { get; set; }

        [JsonIgnore]
        [Version]
        public string Version { get; set; }

        [CreatedAt]
        public DateTime CreatedAt { get; set; }
    }
}