using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.Notification.Dto
{
    public class AzureNotifications
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "deeplink")]
        public string DeepLink { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; } = string.Empty;

        [JsonIgnore]
        [Version]
        public string Version { get; set; }

        [CreatedAt]
        public DateTime CreatedAt { get; set; }
    }
}
