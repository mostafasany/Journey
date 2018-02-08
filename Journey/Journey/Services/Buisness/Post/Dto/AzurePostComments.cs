using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.Post.Dto
{
    public class AzurePostComments
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "post")]
        public string Post { get; set; }


        [JsonProperty(PropertyName = "fname")]
        public string Fname { get; set; }

        [JsonProperty(PropertyName = "lname")]
        public string Lname { get; set; }

        [JsonProperty(PropertyName = "profile")]
        public string Profile { get; set; }

        [Version]
        [JsonIgnore]
        public string Version { get; set; }


        [CreatedAt]
        public DateTime CreatedAt { get; set; }
    }
}