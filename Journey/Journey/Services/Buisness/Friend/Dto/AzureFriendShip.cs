using Journey.Services.Buisness.Account.Dto;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.Friend.Dto
{
    public class AzureFriendShip : AzureAccount
    {
        [JsonProperty(PropertyName = "FriendShipId")]
        public string FriendShipId { get; set; }

        [JsonProperty(PropertyName = "FriendShipStatus")]
        public string FriendShipStatus { get; set; }
    }
}