using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.Account.Entity
{
    public class AzureFriend : AzureAccount
    {
        string friends; //Not Exits in Account table


        [JsonProperty(PropertyName = "friends")]
        public string Friend
        {
            get { return friends; }
            set { friends = value; }
        }
    }

    //public class AzureAccountChallenge : AzureAccount
    //{

    //    string challenge=string.Empty; //Not Exits in Account table


    //    [JsonProperty(PropertyName = "challenge")]
    //    public string Challenge
    //    {
    //        get { return challenge; }
    //        set { challenge = value; }
    //    }
    //}

    public class AzureAccount
    {
        string id = string.Empty;
        string fName = string.Empty;
        string lName = string.Empty;
        string sID = string.Empty;
        string sToken = string.Empty;
        string sProvider = string.Empty;
        string email = string.Empty;
        string gender = string.Empty;
        string status = string.Empty;
        string profile = string.Empty;
        string challenge = string.Empty; //Not Exits in Account table

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "fname")]
        public string FName
        {
            get { return fName; }
            set { fName = value; }
        }

        [JsonProperty(PropertyName = "lName")]
        public string LName
        {
            get { return lName; }
            set { lName = value; }
        }

        [JsonProperty(PropertyName = "status")]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }


        [JsonProperty(PropertyName = "profile")]
        public string Profile
        {
            get { return profile; }
            set { profile = value; }
        }

        [JsonProperty(PropertyName = "sToken")]
        public string SToken
        {
            get { return sToken; }
            set { sToken = value; }
        }


        [JsonProperty(PropertyName = "sID")]
        public string SID
        {
            get { return sID; }
            set { sID = value; }
        }


        [JsonProperty(PropertyName = "sProvider")]
        public string SProvider
        {
            get { return sProvider; }
            set { sProvider = value; }
        }

        [JsonProperty(PropertyName = "email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }


        [JsonProperty(PropertyName = "gender")]
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }



        [JsonProperty(PropertyName = "challenge")]
        public string Challenge
        {
            get { return challenge; }
            set { challenge = value; }
        }


        [JsonIgnore]
        [Version]
        public string Version { get; set; }

        [CreatedAt]
        public DateTime CreatedAt { get; set; }
    }
}
