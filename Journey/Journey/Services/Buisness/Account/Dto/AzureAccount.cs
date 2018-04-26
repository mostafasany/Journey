using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.Account.Dto
{
    public class AzureAccount
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "fname")]
        public string FName { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "lName")]
        public string LName { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; } = string.Empty;


        [JsonProperty(PropertyName = "profile")]
        public string Profile { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "sToken")]
        public string SToken { get; set; } = string.Empty;


        [JsonProperty(PropertyName = "sID")]
        public string SID { get; set; } = string.Empty;


        [JsonProperty(PropertyName = "sProvider")]
        public string SProvider { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; } = string.Empty;


        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; } = string.Empty;


        [JsonProperty(PropertyName = "challenge")]
        public string Challenge { get; set; } = string.Empty;


        [JsonIgnore]
        [Version]
        public string Version { get; set; }

        [CreatedAt]
        public DateTime CreatedAt { get; set; }
    }
}