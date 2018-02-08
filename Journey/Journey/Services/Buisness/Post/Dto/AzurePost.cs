using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.Post.Dto
{
    public class AzurePost
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "images")]
        public string Images { get; set; }


        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "activity")]
        public string Activity { get; set; }

        [JsonProperty(PropertyName = "measurements")]
        public string Measurements { get; set; }

        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }

        [JsonProperty(PropertyName = "points")]
        public double Points { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public int Comments { get; set; }

        [JsonProperty(PropertyName = "likes")]
        public int Likes { get; set; }

        [JsonProperty(PropertyName = "shares")]
        public int Shares { get; set; }

        [JsonProperty(PropertyName = "fname")]
        public string Fname { get; set; }

        [JsonProperty(PropertyName = "lname")]
        public string Lname { get; set; }

        [JsonProperty(PropertyName = "profile")]
        public string Profile { get; set; }

        [JsonProperty(PropertyName = "challenge")]
        public string Challenge { get; set; }

        [Version]
        [JsonIgnore]
        public string Version { get; set; }

        [CreatedAt]
        public DateTime CreatedAt { get; set; }
    }
}