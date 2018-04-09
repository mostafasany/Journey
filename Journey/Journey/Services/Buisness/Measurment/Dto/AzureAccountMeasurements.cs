using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.Measurment.Dto
{
    public class AzureAccountMeasurements
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "weight")]
        public double Weight { get; set; }

        [JsonProperty(PropertyName = "fat")]
        public double Fat { get; set; }

        [JsonProperty(PropertyName = "muscle")]
        public double Muscle { get; set; }


        [JsonProperty(PropertyName = "bmi")]
        public double BMI { get; set; }

        [JsonProperty(PropertyName = "bmr")]
        public double BMR { get; set; }

        [JsonProperty(PropertyName = "water")]
        public double Water { get; set; }

        [JsonProperty(PropertyName = "protein")]
        public double? Protein { get; set; }

        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; }


        [Version]
        public string Version { get; set; }

        [CreatedAt]
        public DateTime CreatedAt { get; set; }
    }
}