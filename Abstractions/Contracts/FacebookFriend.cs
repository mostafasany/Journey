using System.Collections.Generic;

namespace Abstractions.Contracts
{
    public class FacebookFriend
    {
        public string id { get; set; }

        public string name { get; set; }


        public FacebookPictureRoot picture { get; set; }
    }


    public class FacebookFriendRoot
    {
        public List<FacebookFriend> data { get; set; }
    }
}